using AngularDemoAPI.Data;
using AngularDemoAPI.Models.Entities;

namespace AngularDemoAPI.Services.Logging
{
    public class ErrorLogService : IErrorLogService
    {
        private readonly AngularDemoDbContext _context;

        public ErrorLogService(AngularDemoDbContext context)
        {
            _context = context;
        }

        public async Task SaveAsync(ErrorLog log)
        {
            _context.ErrorLog.Add(log);
            await _context.SaveChangesAsync();
        }
    }
}
