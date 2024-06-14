using Hotel.Models.Base;

namespace Hotel.Models
{
    public class RoomImage : BaseEntity
    {
        public string Url { get; set; }
        public int RoomId { get; set; }

        public Room Room { get; set; }
    }
}
