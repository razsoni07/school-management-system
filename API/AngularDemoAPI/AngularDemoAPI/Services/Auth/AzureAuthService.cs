using AngularDemoAPI.Data;
using AngularDemoAPI.Models.ViewModels.Login;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Protocols;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using UserEntity = AngularDemoAPI.Models.Entities.User;

namespace AngularDemoAPI.Services.Auth
{
    public class AzureAuthService : IAzureAuthService
    {
        private readonly IAuthService _authService;
        private readonly IConfiguration _configuration;
        private readonly AngularDemoDbContext _context;

        public AzureAuthService(IAuthService authService, IConfiguration configuration, AngularDemoDbContext context)
        {
            _authService = authService;
            _configuration = configuration;
            _context = context;
        }

        public async Task<LoginResponseDTO> AzureLoginAsync(string idToken)
        {
            var azureAd = _configuration.GetSection("AzureAd");
            var clientId = azureAd["ClientId"]!;

            // Step 1: Fetch Azure's public signing keys
            // "common" endpoint supports both org accounts and personal Microsoft accounts
            var configManager = new ConfigurationManager<OpenIdConnectConfiguration>(
                "https://login.microsoftonline.com/common/v2.0/.well-known/openid-configuration",
                new OpenIdConnectConfigurationRetriever()
            );
            var openIdConfig = await configManager.GetConfigurationAsync();

            // Step 2: Validate the ID token — signature, audience and expiry
            // ValidateIssuer = false because issuer differs per tenant in multi-tenant apps
            var tokenHandler = new JwtSecurityTokenHandler();
            var validationParams = new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = true,
                ValidAudience = clientId,
                ValidateLifetime = true,
                IssuerSigningKeys = openIdConfig.SigningKeys
            };

            var principal = tokenHandler.ValidateToken(idToken, validationParams, out _);

            // Step 3: Extract user info from the validated token claims
            var oid = principal.FindFirst("oid")?.Value ?? principal.FindFirst("http://schemas.microsoft.com/identity/claims/objectidentifier")?.Value;
            var email = principal.FindFirst("preferred_username")?.Value ?? principal.FindFirst("email")?.Value;
            var fullName = principal.FindFirst("name")?.Value ?? email;

            if (string.IsNullOrEmpty(oid) || string.IsNullOrEmpty(email))
                throw new SecurityTokenException("Required claims (oid, email) are missing from the Azure AD token.");

            // Step 4: Find or create the user in the database

            // First try by AzureObjectId — fastest path for returning users
            var user = await _context.Users.Include(u => u.School).FirstOrDefaultAsync(u => u.AzureObjectId == oid);

            if (user == null)
            {
                // Try matching an existing local user by email (account linking)
                user = await _context.Users.Include(u => u.School).FirstOrDefaultAsync(u => u.Email == email);

                if (user != null)
                {
                    // Link their existing account to Azure identity for future logins
                    user.AzureObjectId = oid;
                    await _context.SaveChangesAsync();
                }
            }

            if (user == null)
            {
                // Brand new user — auto-register with Pending role
                var username = email.Split('@')[0];

                user = new UserEntity
                {
                    Username = username,
                    Email = email,
                    FullName = fullName!,
                    Role = "Pending",
                    AzureObjectId = oid,
                    PasswordHash = null,
                    IsActive = true
                };

                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                // Reload with School navigation property after insert
                user = await _context.Users.Include(u => u.School).FirstAsync(u => u.Id == user.Id);
            }

            // Step 5: Issue a local JWT — same shape as the normal login response
            var token = _authService.GenerateJwtToken(user);

            return new LoginResponseDTO
            {
                Token = token,
                ExpiresAt = DateTime.UtcNow.AddMinutes(int.Parse(_configuration["Jwt:ExpiryMinutes"]!))
            };
        }
    }
}
