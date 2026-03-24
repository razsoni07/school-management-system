namespace AngularDemoAPI.Models.ViewModels.Student
{
    public class StudentViewModel
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int? SchoolId { get; set; }
        public string? SchoolName { get; set; }
        public string? SchoolCode { get; set; }
        public string? UserName { get; set; }
        public string RollNumber { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public DateTime DateOfBirth { get; set; }
        public string? Gender { get; set; }
        public int? ClassMasterId { get; set; }
        public string? Class { get; set; }
        public int? SectionId { get; set; }
        public string? Section { get; set; }
        public int? AcademicYearId { get; set; }
        public DateTime AdmissionDate { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
