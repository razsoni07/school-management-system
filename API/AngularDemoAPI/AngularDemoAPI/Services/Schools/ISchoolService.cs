using AngularDemoAPI.Models.ViewModels.School;
using AngularDemoAPI.Models.ViewModels.User;

namespace AngularDemoAPI.Services.Schools
{
    public interface ISchoolService
    {
        Task<List<SchoolViewModel>> GetAllAsync(string role, int? schoolId);
        Task<SchoolViewModel> GetSchoolByIdAsync(int? schoolId);
        Task<SchoolViewModel> ManageSchoolAsync(SchoolRequestModel model, string role, int? userSchoolId);
        Task<bool> DeleteSchoolAsync(int schoolId, string role);
        Task<bool> ToggleSchoolStatus(int id);
    }
}
