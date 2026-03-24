using AngularDemoAPI.Data;
using AngularDemoAPI.Models.Entities;
using AngularDemoAPI.Services.Logging;
using System;
using System.Security.Claims;

namespace AngularDemoAPI.Middlewares
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionMiddleware> _logger;
        private readonly IServiceScopeFactory _scopeFactory;

        public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger, IServiceScopeFactory scopeFactory)
        {
            _next = next;
            _logger = logger;
            _scopeFactory = scopeFactory;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);

                using (var scope = _scopeFactory.CreateScope())
                {
                    var errorService = scope.ServiceProvider.GetRequiredService<IErrorLogService>();

                    int? userId = null;
                    int? schoolId = null;

                    if (context.User.Identity?.IsAuthenticated == true)
                    {
                        var userIdClaim = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                        var schoolIdClaim = context.User.FindFirst("SchoolId")?.Value;

                        if (int.TryParse(userIdClaim, out int uid))
                            userId = uid;

                        if (int.TryParse(schoolIdClaim, out int sid))
                            schoolId = sid;
                    }

                    var log = new ErrorLog
                    {
                        Message = ex.Message,
                        StackTrace = ex.StackTrace,
                        Source = ex.Source,
                        Path = context.Request.Path,
                        Method = context.Request.Method,
                        UserId = userId,
                        SchoolId = schoolId,
                        CreatedAt = DateTime.UtcNow
                    };

                    await errorService.SaveAsync(log);
                }

                context.Response.StatusCode = 500;
                await context.Response.WriteAsJsonAsync(new
                {
                    message = "Internal server error"
                });
            }
        }
    }
}
