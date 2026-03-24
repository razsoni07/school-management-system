using AngularDemoAPI.Models.ViewModels.Student;
using AngularDemoAPI.Services.CurrentUser;
using AngularDemoAPI.Services.Students;
using AngularDemoAPI.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AngularDemoAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]/[Action]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly IStudentService _studentService;
        private readonly ICurrentUserService _currentUser;

        public StudentController(IStudentService studentService, ICurrentUserService currentUser)
        {
            _studentService = studentService;
            _currentUser = currentUser;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllStudents(string role, int? schoolId)
        {
            var result = await _studentService.GetAllAsync(role, schoolId);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetStudentById(int id)
        {
            var student = await _studentService.GetStudentByIdAsync(id);
            if (student == null)
                return NotFound("Student not found");
            return Ok(student);
        }

        [HttpPost]
        public async Task<IActionResult> ManageStudent(StudentRequestModel request)
        {
            var result = await _studentService.ManageAsync(request, _currentUser.Role, _currentUser.SchoolId);
            return Ok(new { message = result });
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteStudent(int id)
        {
            await _studentService.DeleteStudentAsync(id, _currentUser.Role);
            return Ok(new { message = "Student deactivated successfully." });
        }

        [HttpGet]
        public async Task<IActionResult> GetStudentCount(string role, int? schoolId)
        {
            var count = await _studentService.GetStudentCountAsync(role, schoolId);
            return Ok(new { count });
        }

        [HttpPost]
        public async Task<IActionResult> ToggleStudentStatus(int id)
        {
            var result = await _studentService.ToggleStatusAsync(id);
            if (!result) return NotFound();
            return Ok(new { message = "Status updated successfully" });
        }
    }
}
