using AngularDemoAPI.Helpers;

namespace AngularDemoAPI.Models.Entities
{
    public class MeetingParticipants : BaseEntity
    {
        public int Id { get; set; }

        public int MeetingId { get; set; }
        public Meeting Meeting { get; set; } = null!;
        public ParticipantType ParticipantType { get; set; }
        public int ReferenceId { get; set; }
    }
}
