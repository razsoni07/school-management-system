using AngularDemoAPI.Models.ViewModels.Login;

namespace AngularDemoAPI.Services.Auth
{
    public interface IAzureAuthService
    {
        Task<LoginResponseDTO> AzureLoginAsync(string idToken);
    }
}
