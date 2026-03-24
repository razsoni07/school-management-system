using AngularDemoAPI.Data;
using AngularDemoAPI.Helpers;
using AngularDemoAPI.Models.Entities;
using AngularDemoAPI.Models.ViewModels.Student;
using AngularDemoAPI.Services.User;
using Microsoft.EntityFrameworkCore;

namespace AngularDemoAPI.Services.Students
{
    public class StudentService : IStudentService
    {
        private readonly AngularDemoDbContext _context;
        private readonly IUserService _userService;

        public StudentService(AngularDemoDbContext context, IUserService userService)
        {
            _context = context;
            _userService = userService;
        }

        public async Task<List<StudentViewModel>> GetAllAsync(string role, int? schoolId)
        {
            IQueryable<Student> query = _context.Students.AsNoTracking()
                .Include(s => s.User)
                .Include(s => s.School)
                .Include(s => s.Enrollments.Where(e => e.IsActive))
                    .ThenInclude(e => e.ClassMaster)
                .Include(s => s.Enrollments.Where(e => e.IsActive))
                    .ThenInclude(e => e.Section);

            if (role != UserRole.SuperAdmin.ToString())
            {
                query = query.Where(s => s.SchoolId == schoolId);
            }

            var students = await query.ToListAsync();
            return students.Select(MapToViewModel).ToList();
        }

        public async Task<StudentViewModel?> GetStudentByIdAsync(int id)
        {
            var student = await _context.Students.AsNoTracking()
                .Include(s => s.User)
                .Include(s => s.School)
                .Include(s => s.Enrollments.Where(e => e.IsActive))
                    .ThenInclude(e => e.ClassMaster)
                .Include(s => s.Enrollments.Where(e => e.IsActive))
                    .ThenInclude(e => e.Section)
                .FirstOrDefaultAsync(s => s.Id == id);

            return student == null ? null : MapToViewModel(student);
        }

        public async Task<StudentViewModel> ManageAsync(StudentRequestModel model, string role, int? userSchoolId)
        {
            if (model == null) throw new ArgumentNullException(nameof(model));

            Student student;

            if (model.Id == null || model.Id == 0)
            {
                if (role != UserRole.SuperAdmin.ToString() && role != UserRole.SchoolAdmin.ToString() && role != UserRole.Principal.ToString())
                    throw new UnauthorizedAccessException("Not allowed to create student.");

                bool rollExists = await _context.Students.AnyAsync(s => s.RollNumber == model.RollNumber && s.SchoolId == model.SchoolId);
                if (rollExists)
                    throw new Exception("Roll number already exists for this school.");

                int userId = model.UserId;
                if (userId == 0)
                {
                    var userRequest = new AngularDemoAPI.Models.ViewModels.User.UserRequestModel
                    {
                        FullName = model.FullName ?? string.Empty,
                        UserName = model.UserName ?? (model.RollNumber ?? Guid.NewGuid().ToString()),
                        Password = model.Password ?? "ChangeMe123!",
                        Role = UserRole.Student.ToString(),
                        Email = model.Email ?? string.Empty,
                        Phone = model.Phone ?? string.Empty,
                        SchoolId = model.SchoolId ?? 0,
                        IsActive = model.IsActive
                    };
                    userId = await _userService.CreateUserForRoleAsync(userRequest, UserRole.Student.ToString());
                }

                student = new Student
                {
                    UserId = userId,
                    SchoolId = model.SchoolId,
                    RollNumber = model.RollNumber,
                    FullName = model.FullName,
                    DateOfBirth = model.DateOfBirth,
                    Gender = model.Gender,
                    AdmissionDate = model.AdmissionDate,
                    Email = model.Email,
                    Phone = model.Phone,
                    Address = model.Address,
                    IsActive = model.IsActive
                };

                _context.Students.Add(student);
                await _context.SaveChangesAsync(); // Save student first to get the Id

                _context.StudentEnrollment.Add(new StudentEnrollment
                {
                    StudentId = student.Id,
                    ClassMasterId = model.ClassMasterId,
                    SectionId = model.SectionId > 0 ? model.SectionId : null,
                    AcademicYearId = model.AcademicYearId,
                    IsActive = true
                });
                await _context.SaveChangesAsync();

                return MapToViewModel(student);
            }
            else
            {
                student = await _context.Students.FirstOrDefaultAsync(s => s.Id == model.Id);
                if (student == null) throw new Exception("Student not found.");

                if (role != UserRole.SuperAdmin.ToString() && role != UserRole.SchoolAdmin.ToString() && role != UserRole.Principal.ToString())
                    throw new UnauthorizedAccessException("Not allowed to update student.");

                // Student Updated.
                student.SchoolId = model.SchoolId;
                student.RollNumber = model.RollNumber;
                student.FullName = model.FullName;
                student.DateOfBirth = model.DateOfBirth;
                student.Gender = model.Gender;
                student.AdmissionDate = model.AdmissionDate;
                student.Email = model.Email;
                student.Phone = model.Phone;
                student.Address = model.Address;
                student.IsActive = model.IsActive;

                // Student Enrollment Updated.
                var activeEnrollment = await _context.StudentEnrollment.FirstOrDefaultAsync(e => e.StudentId == student.Id && e.IsActive);

                if (activeEnrollment != null)
                {
                    activeEnrollment.ClassMasterId = model.ClassMasterId;
                    activeEnrollment.SectionId = model.SectionId > 0 ? model.SectionId : null;
                    activeEnrollment.AcademicYearId = model.AcademicYearId;
                    activeEnrollment.IsActive = model.IsActive;
                }
                else
                {
                    _context.StudentEnrollment.Add(new StudentEnrollment
                    {
                        StudentId = student.Id,
                        ClassMasterId = model.ClassMasterId,
                        SectionId = model.SectionId > 0 ? model.SectionId : null,
                        AcademicYearId = model.AcademicYearId,
                        IsActive = true
                    });
                }

                await _context.SaveChangesAsync();

                return MapToViewModel(student);
            }
        }

        public async Task<bool> DeleteStudentAsync(int id, string role)
        {
            if (role != UserRole.SuperAdmin.ToString() && role != UserRole.SchoolAdmin.ToString())
                throw new UnauthorizedAccessException("Not allowed to delete student.");

            var student = await _context.Students.FirstOrDefaultAsync(s => s.Id == id);
            if (student == null) throw new Exception("Student not found.");

            student.IsActive = false;
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<int> GetStudentCountAsync(string role, int? schoolId)
        {
            var query = _context.Students.Where(s => s.IsActive);
            if (role != UserRole.SuperAdmin.ToString())
                query = query.Where(s => s.SchoolId == schoolId);
            return await query.CountAsync();
        }

        public async Task<bool> ToggleStatusAsync(int id)
        {
            var student = await _context.Students.FirstOrDefaultAsync(s => s.Id == id);
            if (student == null) return false;

            student.IsActive = !student.IsActive;
            await _context.SaveChangesAsync();

            return true;
        }

        private StudentViewModel MapToViewModel(Student student)
        {
            var activeEnrollment = student.Enrollments?.FirstOrDefault(e => e.IsActive);

            return new StudentViewModel
            {
                Id = student.Id,
                UserId = student.UserId,
                SchoolId = student.SchoolId,
                SchoolName = student.School?.SchoolName,
                SchoolCode = student.School?.Code,
                UserName = student.User?.Username,
                RollNumber = student.RollNumber,
                FullName = student.FullName,
                DateOfBirth = student.DateOfBirth,
                Gender = student.Gender,
                SectionId = activeEnrollment?.SectionId,
                Section = activeEnrollment?.Section?.SectionName,
                ClassMasterId = activeEnrollment?.ClassMasterId,
                Class = activeEnrollment?.ClassMaster?.Name,
                AcademicYearId = activeEnrollment?.AcademicYearId,
                AdmissionDate = student.AdmissionDate,
                Email = student.Email,
                Phone = student.Phone,
                Address = student.Address,
                IsActive = student.IsActive,
                CreatedDate = student.CreatedDate
            };
        }
    }
}
