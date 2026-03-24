using AngularDemoAPI.Models.ViewModels.School;
using AngularDemoAPI.Services.CurrentUser;
using AngularDemoAPI.Services.Schools;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AngularDemoAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]/[Action]")]
    [ApiController]
    public class SchoolController : ControllerBase
    {
        private readonly ISchoolService _schoolService;
        private readonly ICurrentUserService _currentUser;

        public SchoolController(ISchoolService schoolService, ICurrentUserService currentUser)
        {
            _schoolService = schoolService;
            _currentUser = currentUser;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllSchools(string role, int? schoolId)
        {
            var result = await _schoolService.GetAllAsync(role, schoolId);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetSchoolById(int id)
        {
            var school = await _schoolService.GetSchoolByIdAsync(id);

            if (school == null)
                return NotFound("School not found");

            return Ok(school);
        }

        [HttpPost]
        public async Task<IActionResult> ManageSchool(SchoolRequestModel request)
        {
            var result = await _schoolService.ManageSchoolAsync(request, _currentUser.Role, _currentUser.UserId);
            return Ok(new { message = result });
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteSchool(int id)
        {
            await _schoolService.DeleteSchoolAsync(id, _currentUser.Role);
            return Ok(new { message = "School deactivated successfully." });
        }

        [HttpPost]
        public async Task<IActionResult> ToggleSchoolStatus(int id)
        {
            var result = await _schoolService.ToggleSchoolStatus(id);
            if (!result) return NotFound();

            return Ok(new { message = "Status updated successfully" });
        }
    }
}
