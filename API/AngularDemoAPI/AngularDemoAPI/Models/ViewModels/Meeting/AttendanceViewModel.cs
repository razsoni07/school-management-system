namespace AngularDemoAPI.Models.ViewModels.Meeting
{
    public class AttendanceViewModel
    {
        public int UserId { get; set; }
        public string? UserFullName { get; set; }
        public string? UserRole { get; set; }

        public bool IsNotificationSeen { get; set; }
        public DateTime? NotificationSeenAt { get; set; }

        public bool IsPresent { get; set; }
        public DateTime? JoinTime { get; set; }
        public DateTime? LeaveTime { get; set; }
    }
}
