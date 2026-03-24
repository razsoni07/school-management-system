namespace AngularDemoAPI.Models.Entities
{
    public class MeetingAttendance : BaseEntity
    {
        public int Id { get; set; }

        public int MeetingId { get; set; }
        public Meeting Meeting { get; set; } = null!;

        public int UserId { get; set; }
        public User User { get; set; } = null!;

        // Notification tracking
        public bool IsNotificationSeen { get; set; } = false;
        public DateTime? NotificationSeenAt { get; set; }

        // Attendance tracking
        public bool IsPresent { get; set; } = false;
        public DateTime? JoinTime { get; set; }
        public DateTime? LeaveTime { get; set; }
    }
}
