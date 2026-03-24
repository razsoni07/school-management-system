using System.Security.Claims;

namespace AngularDemoAPI.Services.CurrentUser
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        private ClaimsPrincipal User => _httpContextAccessor.HttpContext?.User;
        public int? UserId => int.TryParse(User?.FindFirst(ClaimTypes.NameIdentifier)?.Value, out var id) ? id : null;
        public string Username => User?.FindFirst(ClaimTypes.Name)?.Value;
        public string Role => User?.FindFirst(ClaimTypes.Role)?.Value;
        public int? SchoolId => int.TryParse(User?.FindFirst("schoolId")?.Value, out var schoolId) ? schoolId : null;
        public bool IsAuthenticated => User?.Identity?.IsAuthenticated ?? false;

    }
}
