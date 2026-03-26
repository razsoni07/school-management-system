using System.ComponentModel.DataAnnotations;

namespace AngularDemoAPI.Models.Entities
{
    public class User : BaseEntity
    {
        public int Id { get; set; }
        public int? SchoolId { get; set; }

        [Required]
        [MaxLength(50)]
        public string Username { get; set; } = null!;

        [MaxLength(256)]
        public string? PasswordHash { get; set; } = null!;

        [Required]
        [MaxLength(50)]
        public string Role { get; set; } = null!;

        [Required]
        [MaxLength(100)]
        public string FullName { get; set; } = null!;

        [Required]
        [MaxLength(256)]
        public string Email { get; set; } = null!;

        [MaxLength(20)]
        public string? Phone { get; set; }

        public bool IsActive { get; set; }

        [MaxLength(128)]
        public string? AzureObjectId { get; set; } = null!;
        public School? School { get; set; }
    }
}
