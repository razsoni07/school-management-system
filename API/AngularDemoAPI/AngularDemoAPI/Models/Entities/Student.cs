using System.ComponentModel.DataAnnotations;

namespace AngularDemoAPI.Models.Entities
{
    public class Student : BaseEntity
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public int? SchoolId { get; set; }

        [Required]
        [MaxLength(20)]
        public string RollNumber { get; set; } = null!;

        [Required]
        [MaxLength(100)]
        public string FullName { get; set; } = null!;

        public DateTime DateOfBirth { get; set; }

        [MaxLength(10)]
        public string? Gender { get; set; }

        public DateTime AdmissionDate { get; set; }

        [EmailAddress]
        [MaxLength(256)]
        public string? Email { get; set; }

        [MaxLength(20)]
        public string? Phone { get; set; }

        [MaxLength(300)]
        public string? Address { get; set; }

        public bool IsActive { get; set; } = true;

        public User User { get; set; } = null!;
        public School School { get; set; } = null!;
        public ICollection<StudentEnrollment> Enrollments { get; set; } = new List<StudentEnrollment>();
    }
}
