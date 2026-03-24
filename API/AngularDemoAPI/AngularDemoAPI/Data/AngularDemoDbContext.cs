using AngularDemoAPI.Controllers;
using AngularDemoAPI.Models.Domain;
using AngularDemoAPI.Models.Entities;
using AngularDemoAPI.Services.CurrentUser;
using Microsoft.EntityFrameworkCore;

namespace AngularDemoAPI.Data
{
    public class AngularDemoDbContext : DbContext
    {
        private readonly ICurrentUserService _currentUser;
        public AngularDemoDbContext(DbContextOptions<AngularDemoDbContext> options, ICurrentUserService currentUser) : base(options)
        {
            _currentUser = currentUser;
        }
        public DbSet<User> Users { get; set; }
        public DbSet<School> School { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<ErrorLog> ErrorLog { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Meeting> Meetings { get; set; }
        public DbSet<MeetingParticipants> MeetingParticipants { get; set; }
        public DbSet<MeetingAttendance> MeetingAttendance { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<AcademicYear> AcademicYear { get; set; }
        public DbSet<ClassMaster> ClassMaster { get; set; }
        public DbSet<Section> Section { get; set; }
        public DbSet<StudentEnrollment> StudentEnrollment { get; set; }
        public DbSet<Department> Department { get; set; }
        public DbSet<Roles> Roles { get; set; }

        public override int SaveChanges()
        {
            ApplyAuditInformation();
            return base.SaveChanges();
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            ApplyAuditInformation();
            return await base.SaveChangesAsync(cancellationToken);
        }

        private void ApplyAuditInformation()
        {
            var userId = _currentUser.UserId;
            var entries = ChangeTracker.Entries<BaseEntity>();

            foreach (var entry in entries)
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.CreatedBy = userId;
                    entry.Entity.CreatedDate = DateTime.UtcNow;
                }

                if (entry.State == EntityState.Modified)
                {
                    entry.Entity.UpdatedBy = userId;
                    entry.Entity.UpdatedDate = DateTime.UtcNow;
                }
            }
        }
    }
}
