using Hotel.Models.Base;

namespace Hotel.Models
{
    public class Customer :BaseEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
        
        // Relation to Reservation
        public ICollection<Reservation> Reservations { get; set; }
        public string AppUserId { get; set; }
        public AppUser AppUser { get; set; }
    }
}
