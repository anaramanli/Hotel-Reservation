using Hotel.Models.Base;

namespace Hotel.Models
{
    public class Comment : BaseEntity
    {
        public string Content { get; set; }
        public string UserName { get; set; }

        //Relation
        public int RoomId { get; set; }
        public Room Room { get; set; }

        // Relation to AppUser
        public string UserId { get; set; }
        public AppUser AppUser { get; set; }

    }
}
