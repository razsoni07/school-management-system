using AngularDemoAPI.Models.ViewModels.User;

namespace AngularDemoAPI.Services.User
{
    public interface IUserService
    {
        List<UserViewModel> GetAllUsers(int loggedInUserId, string role);
        UserViewModel GetUserById(int id);
        string ManageUser(UserRequestModel request, int loggedInUserId);
        string DeleteUser(int id);
        Task<int> CreateUserForRoleAsync(UserRequestModel model, string role);
    }
}
