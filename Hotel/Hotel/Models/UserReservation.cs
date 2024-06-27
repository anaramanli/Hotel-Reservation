namespace Hotel.Models
{
	public class UserReservation
	{
		public string AppUserId { get; set; }
		public int ReservationId { get; set; }

		public AppUser AppUser { get; set; }
        public Reservation Reservation { get; set; }
    }
}
