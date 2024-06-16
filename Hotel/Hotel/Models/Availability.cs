using Hotel.Models.Base;

namespace Hotel.Models
{
    public class Availability : BaseEntity
    {
        public int RoomId { get; set; }
        public Room Room { get; set; } 
        public DateTime Date { get; set; }
        public bool IsAvailable { get; set; }
    }
}
