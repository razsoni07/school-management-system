using AngularDemoAPI.Helpers;

namespace AngularDemoAPI.Models.ViewModels.Meeting
{
    public class MeetingViewModel
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public MeetingType MeetingType { get; set; }
        public string? RoomName { get; set; }
        public string? JoinUrl { get; set; }
        public bool IsActive { get; set; }

        public int SchoolId { get; set; }
        public string? SchoolName { get; set; }

        public DateTime MeetingDate { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }

        public string? CreatedByName { get; set; }

        // Notification state for the requesting user
        public bool IsNotificationSeen { get; set; }
        public DateTime? NotificationSeenAt { get; set; }

        // Attendance state for the requesting user
        public bool IsAttended { get; set; }
        public DateTime? JoinTime { get; set; }
        public DateTime? LeaveTime { get; set; }

        public List<ParticipantViewModel> Participants { get; set; } = new();
    }

    public class ParticipantViewModel
    {
        public int ParticipantId { get; set; }
        public ParticipantType ParticipantType { get; set; }
        public string? TargetName { get; set; }
    }
}
