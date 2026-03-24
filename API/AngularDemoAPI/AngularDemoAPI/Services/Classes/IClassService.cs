using AngularDemoAPI.Models.ViewModels.Class;
using AngularDemoAPI.Models.ViewModels.Student;

namespace AngularDemoAPI.Services.Classes
{
    public interface IClassService
    {
        Task<List<ClassViewModel>> GetAllClasses();
    }
}
