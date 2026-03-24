using static System.Collections.Specialized.BitVector32;

namespace AngularDemoAPI.Models.Entities
{
    public class ClassMaster : BaseEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Section> Sections { get; set; }
    }
}
