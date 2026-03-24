using AngularDemoAPI.Models.ViewModels.Teacher;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AngularDemoAPI.Services.Teachers
{
    public interface ITeacherService
    {
        Task<List<TeacherViewModel>> GetAllAsync(string role, int? schoolId);
        Task<TeacherViewModel> GetTeacherByIdAsync(int id);
        Task<TeacherViewModel> ManageAsync(TeacherRequestModel model, string role, int? userSchoolId);
        Task<bool> DeleteTeacherAsync(int id, string role);
        Task<bool> ToggleStatusAsync(int id);
    }
}
