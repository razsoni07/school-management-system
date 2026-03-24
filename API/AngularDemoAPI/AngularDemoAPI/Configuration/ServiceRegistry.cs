using AngularDemoAPI.Services.AcademicYears;
using AngularDemoAPI.Services.Auth;
using AngularDemoAPI.Services.Classes;
using AngularDemoAPI.Services.Contacts;
using AngularDemoAPI.Services.CurrentUser;
using AngularDemoAPI.Services.Dashboard;
using AngularDemoAPI.Services.Departments;
using AngularDemoAPI.Services.Logging;
using AngularDemoAPI.Services.Meetings;
using AngularDemoAPI.Services.Schools;
using AngularDemoAPI.Services.Sections;
using AngularDemoAPI.Services.Students;
using AngularDemoAPI.Services.Teachers;
using AngularDemoAPI.Services.User;

namespace AngularDemoAPI.Configuration
{
    public static class ServiceRegistry
    {
        public static void AddCustomServices(this IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ISchoolService, SchoolService>();
            services.AddScoped<IErrorLogService, ErrorLogService>();
            services.AddScoped<ICurrentUserService, CurrentUserService>();
            services.AddScoped<IStudentService, StudentService>();
            services.AddScoped<ITeacherService, TeacherService>();
            services.AddScoped<IMeetingService, MeetingService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IContactService, ContactService>();
            services.AddScoped<IDashboardService, DashboardService>();
            services.AddScoped<IDepartmentService, DepartmentService>();
            services.AddScoped<IClassService, ClassService>();
            services.AddScoped<ISectionService, SectionService>();
            services.AddScoped<IAcademicYearService, AcademicYearService>();
        }

        public static void InitStaticClasses(this IServiceProvider serviceProvider)
        {
            var scope = serviceProvider.CreateScope();
        }
    }
}
