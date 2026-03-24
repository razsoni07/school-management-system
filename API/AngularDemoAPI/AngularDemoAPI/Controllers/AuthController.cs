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

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost]
        public IActionResult Login(LoginRequestDTO request)
        {
            var response = _authService.Login(request);

            if (response == null)
                return Unauthorized("Invalid username or password");

            return Ok(response);
        }
    }
}
