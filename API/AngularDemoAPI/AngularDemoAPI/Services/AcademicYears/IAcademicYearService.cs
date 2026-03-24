using AngularDemoAPI.Models.Entities;
using AngularDemoAPI.Models.ViewModels.AcademicYear;
using Microsoft.AspNetCore.Mvc;

namespace AngularDemoAPI.Services.AcademicYears
{
    public interface IAcademicYearService
    {
        Task<List<AcademicYearViewModel>> GetAllAcademicYear();
    }
}
