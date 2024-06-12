using Hotel.Models.Base;

namespace Hotel.Models
{
    public class Room : BaseEntity
    {
        public string RoomNumber { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string ImageUrl { get; set; }
        public TimeOnly CheckIn { get; set; } = new TimeOnly(6,00);
        // Relation
        public Category Category { get; set; }
        public RoomStatus RoomStatus { get; set; }
    }
}
