using Hotel.Models.Base;

namespace Hotel.Models
{
    public class RoomStatus : BaseEntity
	{
        public string StatusName { get; set; }

        //Relation
        public ICollection<Room> rooms { get; set; }
    }
}
