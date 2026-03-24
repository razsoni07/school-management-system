using System.ComponentModel.DataAnnotations;

namespace AngularDemoAPI.Models.ViewModels.Contact
{
    public class AddContactRequestDTO
    {
        [Required]
        [MaxLength(100)]
        public required string Name { get; set; }

        [EmailAddress]
        [MaxLength(256)]
        public string Email { get; set; } = null!;

        [Required]
        [MaxLength(20)]
        public required string Phone { get; set; }

        public bool Favorite { get; set; }
    }
}
