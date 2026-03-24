using System.ComponentModel.DataAnnotations;

namespace AngularDemoAPI.Models.ViewModels.Student
{
    public class StudentRequestModel
    {
        public int? Id { get; set; }

        public int UserId { get; set; }

        public int? SchoolId { get; set; }
        public string? SchoolName { get; set; }

        [Required]
        [MaxLength(100)]
        public string FullName { get; set; } = null!;

        [Required]
        [MaxLength(50)]
        public string UserName { get; set; } = null!;

        [MaxLength(100)]
        public string Password { get; set; } = null!;

        [Required]
        [MaxLength(20)]
        public string RollNumber { get; set; } = null!;

        public DateTime DateOfBirth { get; set; }

        [MaxLength(10)]
        public string? Gender { get; set; }

        public int ClassMasterId { get; set; }

        public int? SectionId { get; set; }
        public int AcademicYearId { get; set; }

        public DateTime AdmissionDate { get; set; }

        [EmailAddress]
        [MaxLength(256)]
        public string? Email { get; set; }

        [MaxLength(20)]
        public string? Phone { get; set; }

        [MaxLength(300)]
        public string? Address { get; set; }

        public bool IsActive { get; set; } = true;
    }
}
