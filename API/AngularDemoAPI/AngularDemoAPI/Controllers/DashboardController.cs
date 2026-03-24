using AngularDemoAPI.Services.Dashboard;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static AngularDemoAPI.Models.ViewModels.Dashboard.DashboardDTO;

namespace AngularDemoAPI.Controllers
{
    [Route("api/[controller]/[Action]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly IDashboardService _dashboardService;

        public DashboardController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<AdminDashboardDto>> GetAdminStats()
        {
            var data = await _dashboardService.GetAdminStatsAsync();
            return Ok(data);
        }

        [HttpGet]
        [Authorize(Roles = "principal")]
        public async Task<ActionResult<PrincipalDashboardDto>> GetPrincipalStats()
        {
            var data = await _dashboardService.GetPrincipalStatsAsync();
            return Ok(data);
        }
    }
}
