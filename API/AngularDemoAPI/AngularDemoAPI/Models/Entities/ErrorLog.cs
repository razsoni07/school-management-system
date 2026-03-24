namespace AngularDemoAPI.Models.Entities
{
    public class ErrorLog : BaseEntity
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public string StackTrace { get; set; }
        public string Source { get; set; }
        public string Path { get; set; }
        public string Method { get; set; }
        public int? UserId { get; set; }
        public int? SchoolId { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
