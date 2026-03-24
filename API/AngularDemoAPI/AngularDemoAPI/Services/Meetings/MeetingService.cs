using AngularDemoAPI.Data;
using AngularDemoAPI.Helpers;
using AngularDemoAPI.Hubs;
using AngularDemoAPI.Models.Entities;
using AngularDemoAPI.Models.ViewModels.Meeting;
using AngularDemoAPI.Services.CurrentUser;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace AngularDemoAPI.Services.Meetings
{
    public class MeetingService : IMeetingService
    {
        private readonly AngularDemoDbContext _context;
        private readonly ICurrentUserService _currentUser;
        private readonly IHubContext<MeetingHub> _hubContext;

        public MeetingService(AngularDemoDbContext context, ICurrentUserService currentUser, IHubContext<MeetingHub> hubContext)
        {
            _context = context;
            _currentUser = currentUser;
            _hubContext = hubContext;
        }

        // ─── Create ──────────────────────────────────────────────────────────

        public async Task<MeetingViewModel> CreateAsync(MeetingRequestModel request, string role, int? userId)
        {
            if (role != UserRole.SuperAdmin.ToString() && role != UserRole.SchoolAdmin.ToString() && role != UserRole.Principal.ToString() && role != UserRole.Teacher.ToString())
                throw new UnauthorizedAccessException("Not allowed to create meeting.");

            var meeting = new Meeting
            {
                Title = request.Title,
                Description = request.Description,
                SchoolId = _currentUser.SchoolId ?? request.SchoolId,
                MeetingType = request.MeetingType,
                MeetingDate = request.MeetingDate,
                StartTime = request.StartTime,
                EndTime = request.EndTime,
                RoomName = request.RoomName,
                IsActive = true
            };

            _context.Meetings.Add(meeting);
            await _context.SaveChangesAsync();

            foreach (var p in request.Participants)
            {
                _context.MeetingParticipants.Add(new MeetingParticipants
                {
                    MeetingId = meeting.Id,
                    ParticipantType = p.ParticipantType,
                    ReferenceId = p.ParticipantType switch
                    {
                        ParticipantType.Section => request.SectionId ?? 0,
                        //ParticipantType.IndividualStudent => request.StudentId ?? 0,
                        //ParticipantType.Department => request.DepartmentId ?? 0,
                        _ => throw new Exception("Invalid participant type")
                    }
                });
            }

            await _context.SaveChangesAsync();

            // Send real-time notification
            await _hubContext.Clients.All.SendAsync("ReceiveMeetingNotification",
            new
            {
                meeting.Id,
                meeting.Title,
                meeting.Description,
                meeting.MeetingDate
            });

            return MapToViewModel(meeting);
        }

        // ─── Queries ─────────────────────────────────────────────────────────

        public async Task<IEnumerable<MeetingViewModel>> GetByTeacherAsync(int teacherId)
        {
            var meetings = await _context.Meetings.AsNoTracking().Include(m => m.School).Include(m => m.Participants).Where(m => m.CreatedBy == teacherId && m.IsActive).OrderByDescending(m => m.MeetingDate).ToListAsync();
            return meetings.Select(MapToViewModel);
        }

        public async Task<IEnumerable<MeetingViewModel>> GetBySchoolAsync(int schoolId)
        {
            var meetings = await _context.Meetings.AsNoTracking().Include(m => m.School).Include(m => m.Participants).Where(m => m.SchoolId == schoolId && m.IsActive).OrderByDescending(m => m.MeetingDate).ToListAsync();
            return meetings.Select(MapToViewModel);
        }

        public async Task<MeetingViewModel?> GetByIdAsync(int id)
        {
            var meeting = await _context.Meetings.AsNoTracking().Include(m => m.School).Include(m => m.Participants).FirstOrDefaultAsync(m => m.Id == id);
            return meeting == null ? null : MapToViewModel(meeting);
        }

        public async Task<IEnumerable<MeetingViewModel>> GetAllMeetingsAsync(string role, int? schoolId)
        {
            var query = _context.Meetings.AsNoTracking().Include(m => m.School).Include(m => m.Participants).Where(m => m.IsActive);

            if (role != UserRole.SuperAdmin.ToString() && schoolId.HasValue)
                query = query.Where(m => m.SchoolId == schoolId.Value);

            var meetings = await query.OrderByDescending(m => m.MeetingDate).ToListAsync();
            return meetings.Select(MapToViewModel);
        }

        // ─── Notifications ────────────────────────────────────────────────────

        public async Task<IEnumerable<MeetingViewModel>> GetMyNotificationsAsync()
        {
            var userId = _currentUser.UserId ?? 0;
            var role = _currentUser.Role;
            var schoolId = _currentUser.SchoolId ?? 0;
            var userRoleId = (int)Enum.Parse<UserRole>(role);

            string? userClass = null;
            string? userDepartment = null;

            if (role == UserRole.Student.ToString())
            {
                var student = await _context.Students.AsNoTracking().FirstOrDefaultAsync(s => s.UserId == userId);
            }
            else if (role == UserRole.Teacher.ToString())
            {
                var teacher = await _context.Teachers.AsNoTracking().FirstOrDefaultAsync(t => t.UserId == userId);
                userDepartment = teacher?.DepartmentId.ToString();
            }

            var meetings = await _context.Meetings.AsNoTracking().Include(m => m.School).Include(m => m.Participants)
                           .Where(m => m.IsActive && m.SchoolId == schoolId &&
                               m.Participants.Any(p =>
                                   (p.ParticipantType == ParticipantType.Teacher && p.ReferenceId == userId) ||
                                   (p.ParticipantType == ParticipantType.Section && p.ReferenceId == userRoleId)
                               ))
                           .OrderByDescending(m => m.MeetingDate)
                           .ToListAsync();

            var meetingIds = meetings.Select(m => m.Id).ToList();
            var attendances = await _context.MeetingAttendance.AsNoTracking().Where(a => meetingIds.Contains(a.MeetingId) && a.UserId == userId).ToListAsync();

            return meetings.Select(m =>
            {
                var att = attendances.FirstOrDefault(a => a.MeetingId == m.Id);
                var vm = MapToViewModel(m);
                vm.IsNotificationSeen = att?.IsNotificationSeen ?? false;
                vm.NotificationSeenAt = att?.NotificationSeenAt;
                vm.IsAttended = att?.IsPresent ?? false;
                vm.JoinTime = att?.JoinTime;
                vm.LeaveTime = att?.LeaveTime;
                return vm;
            });
        }

        public async Task AcknowledgeNotificationAsync(int meetingId)
        {
            var userId = _currentUser.UserId ?? 0;
            var existing = await _context.MeetingAttendance.FirstOrDefaultAsync(a => a.MeetingId == meetingId && a.UserId == userId);

            if (existing == null)
            {
                _context.MeetingAttendance.Add(new MeetingAttendance
                {
                    MeetingId = meetingId,
                    UserId = userId,
                    IsNotificationSeen = true,
                    NotificationSeenAt = DateTime.UtcNow,
                    IsPresent = false
                });
            }
            else if (!existing.IsNotificationSeen)
            {
                existing.IsNotificationSeen = true;
                existing.NotificationSeenAt = DateTime.UtcNow;
            }

            await _context.SaveChangesAsync();
        }

        // ─── Attendance ───────────────────────────────────────────────────────

        public async Task JoinMeetingAsync(int meetingId)
        {
            var userId = _currentUser.UserId ?? 0;

            var existing = await _context.MeetingAttendance.FirstOrDefaultAsync(a => a.MeetingId == meetingId && a.UserId == userId);

            if (existing == null)
            {
                _context.MeetingAttendance.Add(new MeetingAttendance
                {
                    MeetingId = meetingId,
                    UserId = userId,
                    IsNotificationSeen = true,
                    NotificationSeenAt = DateTime.UtcNow,
                    IsPresent = true,
                    JoinTime = DateTime.UtcNow
                });
            }
            else
            {
                if (!existing.IsNotificationSeen)
                {
                    existing.IsNotificationSeen = true;
                    existing.NotificationSeenAt ??= DateTime.UtcNow;
                }

                existing.IsPresent = true;
                existing.JoinTime = DateTime.UtcNow;
            }

            await _context.SaveChangesAsync();
        }

        public async Task LeaveMeetingAsync(int meetingId)
        {
            var userId = _currentUser.UserId ?? 0;
            var existing = await _context.MeetingAttendance.FirstOrDefaultAsync(a => a.MeetingId == meetingId && a.UserId == userId);

            if (existing != null)
            {
                existing.LeaveTime = DateTime.UtcNow;
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<AttendanceViewModel>> GetAttendanceAsync(int meetingId)
        {
            var records = await _context.MeetingAttendance.AsNoTracking().Include(a => a.User).Where(a => a.MeetingId == meetingId).OrderByDescending(a => a.JoinTime).ToListAsync();

            return records.Select(a => new AttendanceViewModel
            {
                UserId = a.UserId,
                UserFullName = a.User?.FullName,
                UserRole = a.User?.Role,
                IsNotificationSeen = a.IsNotificationSeen,
                NotificationSeenAt = a.NotificationSeenAt,
                IsPresent = a.IsPresent,
                JoinTime = a.JoinTime,
                LeaveTime = a.LeaveTime
            });
        }

        private static MeetingViewModel MapToViewModel(Meeting meeting)
        {
            return new MeetingViewModel
            {
                Id = meeting.Id,
                Title = meeting.Title,
                Description = meeting.Description,
                MeetingType = meeting.MeetingType,
                RoomName = meeting.RoomName,
                JoinUrl = $"https://meet.jit.si/{meeting.RoomName}",
                IsActive = meeting.IsActive,
                SchoolId = meeting.SchoolId,
                SchoolName = meeting.School?.SchoolName,
                MeetingDate = meeting.MeetingDate,
                StartTime = meeting.StartTime,
                EndTime = meeting.EndTime,
                Participants = meeting.Participants?.Select(p => new ParticipantViewModel
                {
                    ParticipantId = p.ReferenceId,
                    ParticipantType = p.ParticipantType
                }).ToList() ?? new List<ParticipantViewModel>()
            };
        }
    }
}
