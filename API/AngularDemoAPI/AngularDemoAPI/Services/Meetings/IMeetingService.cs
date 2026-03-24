using AngularDemoAPI.Models.ViewModels.Meeting;

namespace AngularDemoAPI.Services.Meetings
{
    public interface IMeetingService
    {
        Task<MeetingViewModel> CreateAsync(MeetingRequestModel meeting, string role, int? userId);
        Task<IEnumerable<MeetingViewModel>> GetByTeacherAsync(int teacherId);
        Task<IEnumerable<MeetingViewModel>> GetBySchoolAsync(int schoolId);
        Task<MeetingViewModel?> GetByIdAsync(int id);
        Task<IEnumerable<MeetingViewModel>> GetAllMeetingsAsync(string role, int? schoolId);

        // Notifications
        Task<IEnumerable<MeetingViewModel>> GetMyNotificationsAsync();
        Task AcknowledgeNotificationAsync(int meetingId);

        // Attendance
        Task JoinMeetingAsync(int meetingId);
        Task LeaveMeetingAsync(int meetingId);
        Task<IEnumerable<AttendanceViewModel>> GetAttendanceAsync(int meetingId);
    }
}
