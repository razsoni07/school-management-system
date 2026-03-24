using AngularDemoAPI.Models.ViewModels.Student;

namespace AngularDemoAPI.Services.Students
{
    public interface IStudentService
    {
        Task<List<StudentViewModel>> GetAllAsync(string role, int? schoolId);
        Task<StudentViewModel?> GetStudentByIdAsync(int id);
        Task<StudentViewModel> ManageAsync(StudentRequestModel model, string role, int? userSchoolId);
        Task<bool> DeleteStudentAsync(int id, string role);
        Task<bool> ToggleStatusAsync(int id);
    }
}
