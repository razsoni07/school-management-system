using System.ComponentModel.DataAnnotations;

namespace AngularDemoAPI.Models.ViewModels.User
{
    public class UserRequestModel
    {
        public int? UserId { get; set; }

        public int SchoolId { get; set; }

        [Required]
        [MaxLength(100)]
        public string FullName { get; set; } = null!;

        [Required]
        [MaxLength(50)]
        public string UserName { get; set; } = null!;

        [MaxLength(100)]
        public string Password { get; set; } = null!;

        [Required]
        [MaxLength(50)]
        public string Role { get; set; } = null!;

        [Required]
        [EmailAddress]
        [MaxLength(256)]
        public string Email { get; set; } = null!;

        [MaxLength(20)]
        public string Phone { get; set; } = null!;

        public bool IsActive { get; set; } = true;
    }
}
