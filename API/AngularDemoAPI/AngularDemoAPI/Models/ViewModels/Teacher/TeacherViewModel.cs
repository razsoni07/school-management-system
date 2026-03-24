using System;

namespace AngularDemoAPI.Models.ViewModels.Teacher
{
    public class TeacherViewModel
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int? SchoolId { get; set; }
        public string SchoolName { get; set; }
        public string SchoolCode { get; set; }
        public string EmployeeCode { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string UserName { get; set; }
        public int? DepartmentId { get; set; }
        public string Qualification { get; set; }
        public int ExperienceYears { get; set; }
        public DateTime JoiningDate { get; set; }
        public string Subjects { get; set; }
        public string ClassTeacherOf { get; set; }
        public bool IsActive { get; set; }
    }
}
