using AngularDemoAPI.Models.Entities;

namespace AngularDemoAPI.Services.Logging
{
    public interface IErrorLogService
    {
        Task SaveAsync(ErrorLog log);
    }
}
