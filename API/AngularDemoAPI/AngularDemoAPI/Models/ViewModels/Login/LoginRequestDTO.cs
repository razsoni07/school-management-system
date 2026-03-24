using System.ComponentModel.DataAnnotations;

namespace AngularDemoAPI.Models.ViewModels.Login
{
    public class LoginRequestDTO
    {
        [Required]
        [MaxLength(100)]
        public string UserName { get; set; } = null!;

        [Required]
        [MaxLength(100)]
        public string Password { get; set; } = null!;
    }
}
