using Hotel.Models.Base;

namespace Hotel.Models
{
	public class ReservationService : BaseEntity
	{
		public int Quantity { get; set; }

		// Relation
		public Reservation Reservation { get; set; }
		public Service Service { get; set; }
	}
}
