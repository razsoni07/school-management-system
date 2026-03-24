using AngularDemoAPI.Data;
using AngularDemoAPI.Models.Entities;
using AngularDemoAPI.Models.ViewModels.Teacher;
using AngularDemoAPI.Services.User;
using Microsoft.EntityFrameworkCore;
using AngularDemoAPI.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AngularDemoAPI.Services.Teachers
{
    public class TeacherService : ITeacherService
    {
        private readonly AngularDemoDbContext _context;

        private readonly IUserService _userService;
        public TeacherService(AngularDemoDbContext context, IUserService userService)
        {
            _context = context;
            _userService = userService;
        }

        public async Task<List<TeacherViewModel>> GetAllAsync(string role, int? schoolId)
        {
            IQueryable<Teacher> query = _context.Teachers.AsNoTracking().Include(t => t.User).Include(t => t.School);
            if (role != UserRole.SuperAdmin.ToString())
            {
                query = query.Where(t => t.SchoolId == schoolId);
            }
            var teachers = await query.ToListAsync();
            return teachers.Select(MapToViewModel).ToList();
        }

        public async Task<TeacherViewModel> GetTeacherByIdAsync(int id)
        {
            var teacher = await _context.Set<Teacher>().AsNoTracking().Include(t => t.User).Include(t => t.School).FirstOrDefaultAsync(t => t.Id == id);
            return teacher == null ? null : MapToViewModel(teacher);
        }

        public async Task<TeacherViewModel> ManageAsync(TeacherRequestModel model, string role, int? userSchoolId)
        {
            if (model == null) throw new ArgumentNullException(nameof(model));
            Teacher teacher;
            if (model.Id == null || model.Id == 0)
            {
                if (role != UserRole.SuperAdmin.ToString() && role != UserRole.SchoolAdmin.ToString() && role != UserRole.Principal.ToString())
                    throw new UnauthorizedAccessException("Not allowed to create teacher.");

                int userId = model.UserId;
                if (userId == 0)
                {
                    // Create user for teacher using all provided fields
                    var userRequest = new AngularDemoAPI.Models.ViewModels.User.UserRequestModel
                    {
                        FullName = model.FullName ?? string.Empty,
                        UserName = model.UserName ?? (model.EmployeeCode ?? Guid.NewGuid().ToString()),
                        Password = model.Password ?? "ChangeMe123!",
                        Role = UserRole.Teacher.ToString(),
                        Email = model.Email ?? string.Empty,
                        Phone = model.Phone ?? string.Empty,
                        SchoolId = model.SchoolId ?? 0,
                        IsActive = model.IsActive
                    };
                    userId = await _userService.CreateUserForRoleAsync(userRequest, UserRole.Teacher.ToString());
                }

                teacher = new Teacher
                {
                    UserId = userId,
                    SchoolId = model.SchoolId,
                    EmployeeCode = model.EmployeeCode,
                    DepartmentId = model.DepartmentId,
                    Qualification = model.Qualification,
                    ExperienceYears = model.ExperienceYears,
                    JoiningDate = model.JoiningDate,
                    Subjects = model.Subjects,
                    ClassTeacherOf = model.ClassTeacherOf,
                    IsActive = model.IsActive
                };
                _context.Set<Teacher>().Add(teacher);
                await _context.SaveChangesAsync();
                return MapToViewModel(teacher);
            }
            else
            {
                teacher = await _context.Set<Teacher>().FirstOrDefaultAsync(t => t.Id == model.Id);
                if (teacher == null) throw new Exception("Teacher not found.");

                if (role != UserRole.SuperAdmin.ToString() && role != UserRole.SchoolAdmin.ToString() && role != UserRole.Principal.ToString())
                    throw new UnauthorizedAccessException("Not allowed to update teacher.");

                teacher.SchoolId = model.SchoolId;
                teacher.EmployeeCode = model.EmployeeCode;
                teacher.DepartmentId = model.DepartmentId;
                teacher.Qualification = model.Qualification;
                teacher.ExperienceYears = model.ExperienceYears;
                teacher.JoiningDate = model.JoiningDate;
                teacher.Subjects = model.Subjects;
                teacher.ClassTeacherOf = model.ClassTeacherOf;
                teacher.IsActive = model.IsActive;

                await _context.SaveChangesAsync();
                return MapToViewModel(teacher);
            }
        }

        public async Task<bool> DeleteTeacherAsync(int id, string role)
        {
            if (role != UserRole.SuperAdmin.ToString() && role != UserRole.SchoolAdmin.ToString())
                throw new UnauthorizedAccessException("Not allowed to delete teacher.");
            var teacher = await _context.Set<Teacher>().FirstOrDefaultAsync(t => t.Id == id);
            if (teacher == null) throw new Exception("Teacher not found.");
            teacher.IsActive = false;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ToggleStatusAsync(int id)
        {
            var teacher = await _context.Set<Teacher>().FirstOrDefaultAsync(t => t.Id == id);
            if (teacher == null) return false;
            teacher.IsActive = !teacher.IsActive;
            await _context.SaveChangesAsync();
            return true;
        }

        private TeacherViewModel MapToViewModel(Teacher teacher)
        {
            return new TeacherViewModel
            {
                Id = teacher.Id,
                UserId = teacher.UserId,
                SchoolId = teacher.SchoolId,
                SchoolName = teacher.School?.SchoolName,
                SchoolCode = teacher.School?.Code,
                EmployeeCode = teacher.EmployeeCode,
                DepartmentId = teacher.DepartmentId,
                Qualification = teacher.Qualification,
                ExperienceYears = teacher.ExperienceYears,
                JoiningDate = teacher.JoiningDate,
                Subjects = teacher.Subjects,
                ClassTeacherOf = teacher.ClassTeacherOf,
                IsActive = teacher.IsActive,
                FullName = teacher.User?.FullName,
                Email = teacher.User?.Email,
                Phone = teacher.User?.Phone,
                UserName = teacher.User?.Username
            };
        }
    }
}
