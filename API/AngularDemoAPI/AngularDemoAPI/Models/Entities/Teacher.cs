using System;
using System.ComponentModel.DataAnnotations;

namespace AngularDemoAPI.Models.Entities
{
    public class Teacher : BaseEntity
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int? SchoolId { get; set; }

        [Required]
        [MaxLength(20)]
        public string EmployeeCode { get; set; } = null!;
        public int? DepartmentId { get; set; }

        [MaxLength(200)]
        public string Qualification { get; set; } = null!;
        public int ExperienceYears { get; set; }
        public DateTime JoiningDate { get; set; }

        [MaxLength(500)]
        public string Subjects { get; set; } = null!;

        [MaxLength(50)]
        public string ClassTeacherOf { get; set; } = null!;
        public bool IsActive { get; set; }
        public User User { get; set; } = null!;
        public School School { get; set; } = null!;
    }
}
