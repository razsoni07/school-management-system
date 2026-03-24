using AngularDemoAPI.Data;
using AngularDemoAPI.Models.ViewModels.Department;
using Microsoft.EntityFrameworkCore;

namespace AngularDemoAPI.Services.Departments
{
    public class DepartmentService : IDepartmentService
    {
        private readonly AngularDemoDbContext _context;

        public DepartmentService(AngularDemoDbContext context)
        {
            _context = context;
        }

        public async Task<List<DepartmentViewModel>> GetAllDepartmentsAsync()
        {
            return await _context.Database
                .SqlQueryRaw<DepartmentViewModel>("EXEC GetAllDepartments")
                .ToListAsync();
        }
    }
}
