using AngularDemoAPI.Models.ViewModels.User;
using AngularDemoAPI.Services.CurrentUser;
using AngularDemoAPI.Services.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AngularDemoAPI.Controllers
{
    [Authorize(Roles = nameof(Helpers.UserRole.SuperAdmin))]
    [Route("api/[controller]/[Action]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ICurrentUserService _currentUser;

        public UserController(IUserService userService, ICurrentUserService currentUser)
        {
            _userService = userService;
            _currentUser = currentUser;
        }

        [HttpGet]
        public IActionResult GetAllUsers()
        {
            var users = _userService.GetAllUsers(_currentUser.UserId ?? 0, _currentUser.Role);
            return Ok(users);
        }

        [HttpGet]
        public IActionResult GetUserById(int id)
        {
            var user = _userService.GetUserById(id);
            if (user == null)
                return NotFound("User not found");

            return Ok(user);
        }

        [HttpPost]
        public IActionResult ManageUser(UserRequestModel request)
        {
            var result = _userService.ManageUser(request, _currentUser.UserId ?? 0);
            return Ok(new { message = result });
        }

        [HttpDelete]
        public IActionResult DeleteUser(int id)
        {
            var result = _userService.DeleteUser(id);
            return Ok(new { message = result });
        }
    }
}
