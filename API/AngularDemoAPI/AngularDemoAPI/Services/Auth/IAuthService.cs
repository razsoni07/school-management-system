using AngularDemoAPI.Models.ViewModels.Login;
using UserEntity = AngularDemoAPI.Models.Entities.User;

namespace AngularDemoAPI.Services.Auth
{
    public interface IAuthService
    {
        LoginResponseDTO? Login(LoginRequestDTO request);
        string GenerateJwtToken(UserEntity user);
    }
}
