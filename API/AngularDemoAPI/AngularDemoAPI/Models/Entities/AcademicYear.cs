using static System.Collections.Specialized.BitVector32;

namespace AngularDemoAPI.Models.Entities
{
    public class AcademicYear : BaseEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsActive { get; set; }
        public ICollection<Section> Sections { get; set; }
        public ICollection<Student> StudentEnrollments { get; set; }
        public ICollection<Meeting> Meetings { get; set; }
    }
}
