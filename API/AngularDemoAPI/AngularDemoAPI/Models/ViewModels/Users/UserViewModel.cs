namespace AngularDemoAPI.Models.ViewModels.User
{
    public class UserViewModel
    {
        public int Id { get; set; }
        public int? SchoolId { get; set; }
        public string FullName { get; set; }
        public string UserName { get; set; }
        public string Role { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
