using AngularDemoAPI.Models.ViewModels.Section;
using AngularDemoAPI.Services.Sections;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AngularDemoAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]/[Action]")]
    [ApiController]
    public class SectionController : ControllerBase
    {
        private readonly ISectionService _sectionService;

        public SectionController(ISectionService sectionService)
        {
            _sectionService = sectionService;
        }

        [HttpGet]
        public async Task<ActionResult> GetSectionsByClass(int classId)
        {
            var result = await _sectionService.GetSectionsByClass(classId);
            return Ok(result);
        }

        [HttpGet]
        public async Task<ActionResult> GetAvailableSectionsByTeacher(int teacherId, int schoolId)
        {
            var result = await _sectionService.GetAvailableSectionsByTeacher(teacherId, schoolId);
            return Ok(result);
        }
    }
}
