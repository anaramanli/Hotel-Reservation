using Hotel.Models.Base;

namespace Hotel.Models
{
	public class Payment : BaseEntity
	{
		public decimal Amount { get; set; }
		public DateTime PaymentDate { get; set; }
		public string PaymentMethod { get; set; }

		// Relation
		public Reservation Reservation { get; set; }
	}
}
