using AngularDemoAPI.Models.ViewModels.Section;
using Microsoft.AspNetCore.Mvc;

namespace AngularDemoAPI.Services.Sections
{
    public interface ISectionService
    {
        Task<List<SectionViewModel>> GetSectionsByClass(int classId);
        Task<List<SectionViewModel>> GetAvailableSectionsByTeacher(int teacherId, int schoolId);
    }
}
