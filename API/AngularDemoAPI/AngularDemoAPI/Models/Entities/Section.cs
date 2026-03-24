using System.Diagnostics;

namespace AngularDemoAPI.Models.Entities
{
    public class Section : BaseEntity
    {
        public int Id { get; set; }
        public int? SchoolId { get; set; }
        public School? School { get; set; }
        public int ClassMasterId { get; set; }
        public ClassMaster Class { get; set; }
        public string SectionName { get; set; }
        public int AcademicYearId { get; set; }
        public AcademicYear AcademicYear { get; set; }
        public ICollection<Student> Student { get; set; }
    }
}
