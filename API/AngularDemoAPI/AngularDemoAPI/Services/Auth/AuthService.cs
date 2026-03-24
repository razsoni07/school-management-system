using AngularDemoAPI.Data;
using AngularDemoAPI.Models.Entities;
using AngularDemoAPI.Models.ViewModels.Login;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using UserEntity = AngularDemoAPI.Models.Entities.User;

namespace AngularDemoAPI.Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly AngularDemoDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly PasswordHasher<UserEntity> _passwordHasher;

        public AuthService(AngularDemoDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
            _passwordHasher = new PasswordHasher<UserEntity>();
        }

        public LoginResponseDTO? Login(LoginRequestDTO request)
        {
            var user = _context.Users.Include(u => u.School).SingleOrDefault(u => u.Username == request.UserName);

            if (user == null)
                return null;

            var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, request.Password);

            if (result == PasswordVerificationResult.Failed)
                return null;

            var token = GenerateJwtToken(user);

            return new LoginResponseDTO
            {
                Token = token,
                ExpiresAt = DateTime.UtcNow.AddMinutes(int.Parse(_configuration["Jwt:ExpiryMinutes"]!))
            };
        }

        private string GenerateJwtToken(UserEntity user)
        {
            var jwt = _configuration.GetSection("Jwt");
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwt["key"]!)
            );

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.UniqueName, user.Username),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role),
                new Claim(JwtRegisteredClaimNames.GivenName, user.FullName)
            };

            if (user.SchoolId.HasValue)
                claims.Add(new Claim("schoolId", user.SchoolId.Value.ToString()));

            if (user.School != null)
                claims.Add(new Claim("schoolName", user.School.SchoolName));

            var token = new JwtSecurityToken(
                issuer: jwt["Issuer"],
                audience: jwt["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(int.Parse(jwt["ExpiryMinutes"]!)),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
