using AngularDemoAPI.Data;
using AngularDemoAPI.Helpers;
using Microsoft.EntityFrameworkCore;
using static AngularDemoAPI.Models.ViewModels.Dashboard.DashboardDTO;

namespace AngularDemoAPI.Services.Dashboard
{
    public class DashboardService : IDashboardService
    {
        private readonly AngularDemoDbContext _context;

        public DashboardService(AngularDemoDbContext context)
        {
            _context = context;
        }

        public async Task<AdminDashboardDto> GetAdminStatsAsync()
        {
            return new AdminDashboardDto
            {
                TotalUsers = await _context.Users.CountAsync(),
                TotalPrincipals = await _context.Users.CountAsync(u => u.Role == UserRole.Principal.ToString()),
                TotalTeachers = await _context.Users.CountAsync(u => u.Role == UserRole.Teacher.ToString()),
                TotalContacts = await _context.Contacts.CountAsync()
            };
        }

        public async Task<PrincipalDashboardDto> GetPrincipalStatsAsync()
        {
            return new PrincipalDashboardDto
            {
                TotalTeachers = await _context.Users.CountAsync(u => u.Role == UserRole.Teacher.ToString()),
                TotalContacts = await _context.Contacts.CountAsync()
            };
        }
    }
}
