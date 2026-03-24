using AngularDemoAPI.Models.ViewModels.Department;

namespace AngularDemoAPI.Services.Departments
{
    public interface IDepartmentService
    {
        Task<List<DepartmentViewModel>> GetAllDepartmentsAsync();
    }
}
