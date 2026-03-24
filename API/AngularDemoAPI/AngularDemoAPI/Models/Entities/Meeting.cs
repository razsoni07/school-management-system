using AngularDemoAPI.Helpers;
using System.ComponentModel.DataAnnotations;

namespace AngularDemoAPI.Models.Entities
{
    public class Meeting : BaseEntity
    {
        public int Id { get; set; }

        [MaxLength(200)]
        public string? Title { get; set; }

        [MaxLength(1000)]
        public string? Description { get; set; }
        public int SchoolId { get; set; }
        public School? School { get; set; }
        public MeetingType MeetingType { get; set; }
        public DateTime MeetingDate { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }

        [MaxLength(100)]
        public string? RoomName { get; set; }
        public int? AcademicYearId { get; set; }
        public AcademicYear AcademicYear { get; set; }
        public bool IsActive { get; set; } = true;
        public ICollection<MeetingParticipants> Participants { get; set; } = new List<MeetingParticipants>();
        public ICollection<MeetingAttendance> Attendances { get; set; } = new List<MeetingAttendance>();
    }
}
