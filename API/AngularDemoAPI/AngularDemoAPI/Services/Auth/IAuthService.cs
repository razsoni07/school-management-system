using AngularDemoAPI.Models.ViewModels.Login;

namespace AngularDemoAPI.Services.Auth
{
    public interface IAuthService
    {
        LoginResponseDTO? Login(LoginRequestDTO request);
    }
}
