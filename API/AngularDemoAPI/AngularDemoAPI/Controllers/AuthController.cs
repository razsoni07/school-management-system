using AngularDemoAPI.Models.ViewModels.Login;
using AngularDemoAPI.Services.Auth;
using Microsoft.AspNetCore.Mvc;

namespace AngularDemoAPI.Controllers
{
    [Route("api/[controller]/[Action]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IAzureAuthService _azureAuthService;

        public AuthController(IAuthService authService, IAzureAuthService azureAuthService)
        {
            _authService = authService;
            _azureAuthService = azureAuthService;
        }

        [HttpPost]
        public IActionResult Login(LoginRequestDTO request)
        {
            var response = _authService.Login(request);

            if (response == null)
                return Unauthorized("Invalid username or password");

            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> AzureLogin(AzureLoginRequestDTO request)
        {
            var response = await _azureAuthService.AzureLoginAsync(request.IdToken);

            if (response == null)
                return Unauthorized("Azure login failed");

            return Ok(response);
        }
    }
}
