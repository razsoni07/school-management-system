namespace AngularDemoAPI.Models.Entities
{
    public class StudentEnrollment : BaseEntity
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public Student Student { get; set; } = null!;
        public int? SectionId { get; set; }
        public int? ClassMasterId { get; set; }
        public ClassMaster? ClassMaster { get; set; }
        public Section? Section { get; set; }
        public int AcademicYearId { get; set; }
        public AcademicYear AcademicYear { get; set; } = null!;
        public bool IsActive { get; set; } = true;
    }
}
