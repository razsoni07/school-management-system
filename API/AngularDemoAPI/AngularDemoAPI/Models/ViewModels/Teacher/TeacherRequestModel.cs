using System;
using System.ComponentModel.DataAnnotations;

namespace AngularDemoAPI.Models.ViewModels.Teacher
{
    public class TeacherRequestModel
    {
        public int? Id { get; set; }

        public int UserId { get; set; }

        public int? SchoolId { get; set; }

        [Required]
        [MaxLength(100)]
        public string FullName { get; set; } = null!;

        [Required]
        [MaxLength(50)]
        public string UserName { get; set; } = null!;

        [MaxLength(100)]
        public string Password { get; set; } = null!;

        [EmailAddress]
        [MaxLength(256)]
        public string Email { get; set; } = null!;

        [MaxLength(20)]
        public string Phone { get; set; } = null!;

        [Required]
        [MaxLength(20)]
        public string EmployeeCode { get; set; } = null!;

        public int? DepartmentId { get; set; }

        [MaxLength(200)]
        public string Qualification { get; set; } = null!;

        [Range(0, 50)]
        public int ExperienceYears { get; set; }

        public DateTime JoiningDate { get; set; }

        [MaxLength(500)]
        public string Subjects { get; set; } = null!;

        [MaxLength(50)]
        public string ClassTeacherOf { get; set; } = null!;

        public bool IsActive { get; set; }
    }
}
