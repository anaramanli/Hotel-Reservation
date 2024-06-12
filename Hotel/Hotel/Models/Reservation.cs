using Hotel.Models.Base;

namespace Hotel.Models
{
    public class Reservation : BaseEntity
    {
        public DateTime CheckOutDate { get; set; }
        public int NumberOfGuests { get; set; }

        // Relation 
        public Room Room { get; set; }
        public Customer Customer { get; set; }

        public ICollection<Payment> Payments { get; set; }
        public ICollection<ReservationService> ReservationServices { get; set; }
	}
}
