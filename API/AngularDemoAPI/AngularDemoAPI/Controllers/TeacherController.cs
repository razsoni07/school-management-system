using AngularDemoAPI.Models.ViewModels.Teacher;
using AngularDemoAPI.Services.CurrentUser;
using AngularDemoAPI.Services.Teachers;
using Microsoft.AspNetCore.Authorization;
using AngularDemoAPI.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace AngularDemoAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]/[Action]")]
    [ApiController]
    public class TeacherController : ControllerBase
    {
        private readonly ITeacherService _teacherService;
        private readonly ICurrentUserService _currentUser;

        public TeacherController(ITeacherService teacherService, ICurrentUserService currentUser)
        {
            _teacherService = teacherService;
            _currentUser = currentUser;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllTeachers(string role, int? schoolId)
        {
            var result = await _teacherService.GetAllAsync(role, schoolId);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetTeacherById(int id)
        {
            var teacher = await _teacherService.GetTeacherByIdAsync(id);
            if (teacher == null)
                return NotFound("Teacher not found");
            return Ok(teacher);
        }

        [HttpPost]
        public async Task<IActionResult> ManageTeacher(TeacherRequestModel request)
        {
            var result = await _teacherService.ManageAsync(request, _currentUser.Role, _currentUser.UserId);
            return Ok(new { message = result });
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteTeacher(int id)
        {
            await _teacherService.DeleteTeacherAsync(id, _currentUser.Role);
            return Ok(new { message = "Teacher deactivated successfully." });
        }

        [HttpPost]
        public async Task<IActionResult> ToggleTeacherStatus(int id)
        {
            var result = await _teacherService.ToggleStatusAsync(id);
            if (!result) return NotFound();
            return Ok(new { message = "Status updated successfully" });
        }
    }
}
