using AngularDemoAPI.Data;
using AngularDemoAPI.Models.Entities;
using AngularDemoAPI.Models.ViewModels.Class;
using Microsoft.EntityFrameworkCore;

namespace AngularDemoAPI.Services.Classes
{
    public class ClassService : IClassService
    {
        private readonly AngularDemoDbContext _context;

        public ClassService(AngularDemoDbContext context)
        {
            _context = context;
        }

        public async Task<List<ClassViewModel>> GetAllClasses()
        {
            IQueryable<ClassMaster> query = _context.ClassMaster.AsNoTracking();

            var classes = await query.ToListAsync();
            return classes.Select(MapToViewModel).ToList();
        }

        private ClassViewModel MapToViewModel(ClassMaster classes)
        {
            return new ClassViewModel
            {
                Id = classes.Id,
                Name = classes.Name,
            };
        }
    }
}
