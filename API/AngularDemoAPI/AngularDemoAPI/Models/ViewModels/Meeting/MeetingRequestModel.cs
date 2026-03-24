using AngularDemoAPI.Helpers;
using System;
using System.ComponentModel.DataAnnotations;

namespace AngularDemoAPI.Models.ViewModels.Meeting
{
    public class MeetingRequestModel
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string Title { get; set; } = null!;

        [MaxLength(1000)]
        public string? Description { get; set; }
        public MeetingType MeetingType { get; set; }
        public int SchoolId { get; set; }
        public int? SectionId { get; set; }

        [Required]
        [MaxLength(100)]
        public string RoomName { get; set; } = null!;

        public bool IsActive { get; set; }

        [Required]
        public DateTime MeetingDate { get; set; }

        public TimeSpan StartTime { get; set; }

        public TimeSpan EndTime { get; set; }
        public List<ParticipantRequestModel> Participants { get; set; }
    }
    public class ParticipantRequestModel
    {
        public int ReferenceId { get; set; }
        public ParticipantType ParticipantType { get; set; }
        public string? TargetName { get; set; }
    }
}
