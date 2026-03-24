using AngularDemoAPI.Models.Entities;

namespace AngularDemoAPI.Models.Domain
{
    public class Contact : BaseEntity
    {
        public int Id { get; set; }

        public required string Name { get; set; }

        public string Email { get; set; }

        public required string Phone { get; set; }

        public bool Favorite { get; set; }

    }
}
