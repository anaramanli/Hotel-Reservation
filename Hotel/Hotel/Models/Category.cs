using Hotel.Models.Base;

namespace Hotel.Models
{
    public class Category : BaseEntity
    {
        public string CategoryName { get; set; }

        // Relation
        public ICollection<Room> rooms { get; set; }
    }
}
