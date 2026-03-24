using static AngularDemoAPI.Models.ViewModels.Dashboard.DashboardDTO;

namespace AngularDemoAPI.Services.Dashboard
{
    public interface IDashboardService
    {
        Task<AdminDashboardDto> GetAdminStatsAsync();
        Task<PrincipalDashboardDto> GetPrincipalStatsAsync();
    }
}
