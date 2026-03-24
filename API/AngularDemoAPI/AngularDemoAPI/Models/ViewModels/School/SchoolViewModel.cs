namespace AngularDemoAPI.Models.ViewModels.School
{
    public class SchoolViewModel
    {
        public int Id { get; set; }
        public string SchoolName { get; set; }
        public string? Code { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedDate { get; set; }
    }
}
