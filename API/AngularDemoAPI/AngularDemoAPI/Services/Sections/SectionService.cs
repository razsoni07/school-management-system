using AngularDemoAPI.Data;
using AngularDemoAPI.Models.Entities;
using AngularDemoAPI.Models.ViewModels.Section;
using AngularDemoAPI.Models.ViewModels.Student;
using AngularDemoAPI.Services.CurrentUser;
using Microsoft.EntityFrameworkCore;

namespace AngularDemoAPI.Services.Sections
{
    public class SectionService : ISectionService
    {
        private readonly AngularDemoDbContext _context;
        private readonly ICurrentUserService _currentUserService;

        public SectionService(AngularDemoDbContext context, ICurrentUserService currentUserService)
        {
            _context = context;
            _currentUserService = currentUserService;
        }

        public async Task<List<SectionViewModel>> GetSectionsByClass(int classId)
        {
            IQueryable<Section> query = _context.Section.AsNoTracking().Where(a => a.ClassMasterId == classId).Include(a => a.Class).Include(a => a.AcademicYear);

            var section = await query.ToListAsync();
            return section.Select(MapToViewModel).ToList();
        }
        public async Task<List<SectionViewModel>> GetAvailableSectionsByTeacher(int teacherId, int schoolId)
        {
            var activeAcademicYear = await _context.AcademicYear.AsNoTracking().FirstOrDefaultAsync(a => a.IsActive);

            if (activeAcademicYear == null) return new List<SectionViewModel>();

            var sections = await _context.Section.AsNoTracking().Where(s => s.SchoolId == schoolId && s.AcademicYearId == activeAcademicYear.Id).Include(s => s.Class).Include(s => s.AcademicYear).ToListAsync();
            return sections.Select(MapToViewModel).ToList();
        }

        private SectionViewModel MapToViewModel(Section section) 
        {
            return new SectionViewModel
            {
                AcademicYearId = section.AcademicYearId,
                ClassMasterId = section.ClassMasterId,
                Id = section.Id,
                SectionName = "Class " + section.Class.Name + " - " + section.SectionName
            };
        }
    }
}
