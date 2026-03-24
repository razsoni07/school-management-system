using AngularDemoAPI.Data;
using AngularDemoAPI.Models.ViewModels.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using UserEntity = AngularDemoAPI.Models.Entities.User;

namespace AngularDemoAPI.Services.User
{
    public class UserService : IUserService
    {
        private readonly AngularDemoDbContext _context;
        private readonly PasswordHasher<UserEntity> _passwordHasher;

        public UserService(AngularDemoDbContext context)
        {
            _context = context;
            _passwordHasher = new PasswordHasher<UserEntity>();
        }

        public List<UserViewModel> GetAllUsers(int loggedInUserId, string role)
        {
            var users = _context.Users.AsNoTracking().ToList();

            return users.Select(u => new UserViewModel
            {
                Id = u.Id,
                FullName = u.FullName,
                UserName = u.Username,
                Role = u.Role,
                Email = u.Email,
                Phone = u.Phone,
                SchoolId = u.SchoolId,
                IsActive = u.IsActive,
                CreatedDate = u.CreatedDate
            }).ToList();
        }

        public UserViewModel GetUserById(int id)
        {
            var u = _context.Users.AsNoTracking().FirstOrDefault(x => x.Id == id);
            if (u == null) return null;

            return new UserViewModel
            {
                Id = u.Id,
                FullName = u.FullName,
                UserName = u.Username,
                Role = u.Role,
                Email = u.Email,
                Phone = u.Phone,
                SchoolId = u.SchoolId,
                IsActive = u.IsActive,
                CreatedDate = u.CreatedDate
            };
        }

        public string ManageUser(UserRequestModel request, int loggedInUserId)
        {
            // CREATE
            if (request.UserId == null || request.UserId == 0)
            {
                if (_context.Users.Any(u => u.Username == request.UserName))
                    return "User already exists";

                var user = new UserEntity
                {
                    FullName = request.FullName,
                    Username = request.UserName,
                    Role = request.Role,
                    Email = request.Email,
                    Phone = request.Phone,
                    SchoolId = request.SchoolId,
                    IsActive = true,
                    CreatedBy = loggedInUserId,
                    CreatedDate = DateTime.UtcNow
                };

                user.PasswordHash = _passwordHasher.HashPassword(user, request.Password);

                _context.Users.Add(user);
                _context.SaveChanges();

                return "User created successfully";
            }
            else
            {
                var existingUser = _context.Users.FirstOrDefault(u => u.Id == request.UserId);
                if (existingUser == null)
                    return "User not found";

                existingUser.FullName = request.FullName;
                existingUser.Username = request.UserName;
                existingUser.Role = request.Role;
                existingUser.Email = request.Email;
                existingUser.Phone = request.Phone;
                existingUser.SchoolId = request.SchoolId;

                existingUser.UpdatedBy = loggedInUserId;
                existingUser.UpdatedDate = DateTime.UtcNow;

                if (!string.IsNullOrEmpty(request.Password))
                {
                    existingUser.PasswordHash =
                        _passwordHasher.HashPassword(existingUser, request.Password);
                }

                _context.SaveChanges();
                return "User updated successfully";
            }
        }

        public string DeleteUser(int id)
        {
            var user = _context.Users.FirstOrDefault(x => x.Id == id);
            if (user == null)
                return "User not found";

            _context.Users.Remove(user);
            _context.SaveChanges();

            return "User deleted successfully";
        }
        public async Task<int> CreateUserForRoleAsync(UserRequestModel model, string role)
        {
            if (_context.Users.Any(u => u.Username == model.UserName))
                throw new Exception("User already exists");

            var user = new UserEntity
            {
                FullName = model.FullName,
                Username = model.UserName,
                Role = role,
                Email = model.Email,
                Phone = model.Phone,
                SchoolId = model.SchoolId,
                IsActive = true,
                CreatedBy = null, // Set by audit
                CreatedDate = DateTime.UtcNow
            };

            user.PasswordHash = _passwordHasher.HashPassword(user, model.Password);

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user.Id;
        }
    }
}
