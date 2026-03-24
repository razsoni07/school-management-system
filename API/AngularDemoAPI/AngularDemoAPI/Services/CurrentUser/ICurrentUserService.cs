namespace AngularDemoAPI.Services.CurrentUser
{
    public interface ICurrentUserService
    {
        int? UserId { get; }
        string Username { get; }
        string Role { get; }
        int? SchoolId { get; }
        bool IsAuthenticated { get; }
    }
}
