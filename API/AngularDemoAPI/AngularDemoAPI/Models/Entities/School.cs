using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AngularDemoAPI.Models.Entities
{
    public class School : BaseEntity
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string SchoolName { get; set; } = null!;

        [MaxLength(20)]
        public string? Code { get; set; }

        [MaxLength(300)]
        public string? Address { get; set; }

        [MaxLength(100)]
        public string? City { get; set; }

        [MaxLength(100)]
        public string? State { get; set; }

        public bool IsActive { get; set; } = true;
        public ICollection<User> Users { get; set; } = new List<User>();
    }
}
