using AngularDemoAPI.Models.Entities;

namespace AngularDemoAPI.Models.ViewModels.Section
{
    public class SectionViewModel
    {
        public int Id { get; set; }
        public int ClassMasterId { get; set; }
        public string SectionName { get; set; }
        public int AcademicYearId { get; set; }
    }
}
