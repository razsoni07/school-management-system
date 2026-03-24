using AngularDemoAPI.Data;
using AngularDemoAPI.Models.Entities;
using AngularDemoAPI.Models.ViewModels.School;
using AngularDemoAPI.Models.ViewModels.User;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AngularDemoAPI.Services.Schools
{
    public class SchoolService : ISchoolService
    {
        private readonly AngularDemoDbContext _context;

        public SchoolService(AngularDemoDbContext context)
        {
            _context = context;
        }
        public async Task<List<SchoolViewModel>> GetAllAsync(string role, int? schoolId)
        {
            if (role == "SuperAdmin")
            {
                return await _context.School
                    .AsNoTracking()
                    .Select(s => new SchoolViewModel
                    {
                        Id = s.Id,
                        SchoolName = s.SchoolName,
                        Code = s.Code,
                        Address = s.Address,
                        City = s.City,
                        State = s.State,
                        IsActive = s.IsActive,
                        CreatedDate = s.CreatedDate
                    }).ToListAsync();
            }
            else
            {
                return await _context.School
                    .AsNoTracking()
                    .Where(s => s.Id == schoolId)
                    .Select(s => new SchoolViewModel
                    {
                        Id = s.Id,
                        SchoolName = s.SchoolName,
                        Code = s.Code,
                        Address = s.Address,
                        City = s.City,
                        State = s.State,
                        IsActive = s.IsActive,
                        CreatedDate = s.CreatedDate
                    }).ToListAsync();
            }
        }

        public async Task<SchoolViewModel> GetSchoolByIdAsync(int? schoolId)
        {
            if (schoolId == null)
                return null;

            return await _context.School
                .AsNoTracking()
                .Where(s => s.Id == schoolId)
                .Select(s => new SchoolViewModel
                {
                    Id = s.Id,
                    SchoolName = s.SchoolName,
                    Code = s.Code,
                    Address = s.Address,
                    City = s.City,
                    State = s.State,
                    IsActive = s.IsActive,
                    CreatedDate = s.CreatedDate
                }).FirstOrDefaultAsync();
        }

        public async Task<SchoolViewModel> ManageSchoolAsync(SchoolRequestModel model, string role, int? userSchoolId)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            if (model.Id == 0)
            {
                if (role != "SuperAdmin")
                    throw new UnauthorizedAccessException("Only SuperAdmin can create schools.");

                if (!string.IsNullOrWhiteSpace(model.Code))
                {
                    bool codeExists = await _context.School.AnyAsync(x => x.Code == model.Code);

                    if (codeExists)
                        throw new Exception("School code already exists.");
                }

                var newSchool = new School
                {
                    SchoolName = model.SchoolName,
                    Code = model.Code,
                    Address = model.Address,
                    City = model.City,
                    State = model.State,
                    IsActive = true
                };

                await _context.School.AddAsync(newSchool);
                await _context.SaveChangesAsync();

                return MapToViewModel(newSchool);
            }

            var existingSchool = await _context.School.FirstOrDefaultAsync(x => x.Id == model.Id);

            if (existingSchool == null)
                throw new Exception("School not found.");

            if (role != "SuperAdmin")
            {
                if (userSchoolId == null || existingSchool.Id != userSchoolId)
                    throw new UnauthorizedAccessException("Not allowed to update this school.");
            }

            existingSchool.SchoolName = model.SchoolName;
            existingSchool.Code = model.Code;
            existingSchool.Address = model.Address;
            existingSchool.City = model.City;
            existingSchool.State = model.State;

            await _context.SaveChangesAsync();

            return MapToViewModel(existingSchool);
        }

        public async Task<bool> DeleteSchoolAsync(int schoolId, string role)
        {
            if (role != "SuperAdmin")
                throw new UnauthorizedAccessException("Only SuperAdmin can delete schools.");

            var school = await _context.School.FirstOrDefaultAsync(x => x.Id == schoolId);

            if (school == null)
                throw new Exception("School not found.");

            if (!school.IsActive)
                throw new Exception("School is already deactivated.");

            // Soft delete
            school.IsActive = false;

            await _context.SaveChangesAsync();

            return true;
        }
        
        public async Task<bool> ToggleSchoolStatus(int id)
        {
            var school = await _context.School.FindAsync(id);
            if (school == null) return false;

            school.IsActive = !school.IsActive;

            _context.School.Update(school);
            await _context.SaveChangesAsync();

            return true;
        }

        private SchoolViewModel MapToViewModel(School school)
        {
            return new SchoolViewModel
            {
                Id = school.Id,
                SchoolName = school.SchoolName,
                Code = school.Code,
                Address = school.Address,
                City = school.City,
                State = school.State,
                IsActive = school.IsActive,
                CreatedDate = school.CreatedDate
            };
        }
    }
}
