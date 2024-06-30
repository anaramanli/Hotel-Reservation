using Hotel.Models.Base;

namespace Hotel.Models
{
    public class Room : BaseEntity
    {
        public string RoomNumber { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int Rating { get; set; }
        public int Beds { get; set; }
        public int Bathrooms { get; set; }
        public string Location { get; set; }
        public TimeOnly CheckIn { get; set; } = new TimeOnly(6,00);
        // Relation
        public int CategoryId { get; set; }
        public int RoomStatusId { get; set; }

        public Category? Category { get; set; }
        public RoomStatus? RoomStatus { get; set; }

        public ICollection<RoomImage> Images { get; set; } = new List<RoomImage>();
        // Availability relation
        public ICollection<Availability> Availabilities { get; set; } = new List<Availability>();
        // Relation to Reservation
        public ICollection<Reservation> Reservations { get; set; }
        // Relation to Comment
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
    }
}
