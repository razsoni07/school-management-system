namespace AngularDemoAPI.Models.ViewModels.Login
{
    public class LoginResponseDTO
    {
        public string Token { get; set; } = null!;
        public DateTime ExpiresAt { get; set; }
    }
}
