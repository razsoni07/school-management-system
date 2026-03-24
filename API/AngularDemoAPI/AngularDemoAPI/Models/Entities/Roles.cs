namespace AngularDemoAPI.Models.Entities
{
    public class Roles : BaseEntity
    {
        public int Id { get; set; }
        public string Role { get; set; }
        public string? Description { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
