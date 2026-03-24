using AngularDemoAPI.Data;
using AngularDemoAPI.Models.Entities;
using AngularDemoAPI.Models.ViewModels.AcademicYear;
using Microsoft.EntityFrameworkCore;

namespace AngularDemoAPI.Services.AcademicYears
{
    public class AcademicYearService : IAcademicYearService
    {
        private readonly AngularDemoDbContext _context;

        public AcademicYearService(AngularDemoDbContext context)
        {
            _context = context;
        }

        public async Task<List<AcademicYearViewModel>> GetAllAcademicYear()
        {
            IQueryable<AcademicYear> query = _context.AcademicYear.AsNoTracking();

            var academicYear = await query.ToListAsync();
            return academicYear.Select(MapToViewModel).ToList();
        }

        private AcademicYearViewModel MapToViewModel(AcademicYear AcademicYear)
        {
            return new AcademicYearViewModel
            {
                Id = AcademicYear.Id,
                Name = AcademicYear.Name,
                StartDate = AcademicYear.StartDate,
                EndDate = AcademicYear.EndDate,
                IsActive = AcademicYear.IsActive
            };
        }
    }
}
