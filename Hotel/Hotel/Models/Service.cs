using Hotel.Models.Base;

namespace Hotel.Models
{
	public class Service : BaseEntity
	{
		public string ServiceName { get; set; }
		public string Description { get; set; }
		public decimal Price { get; set; }

		// Relation
		public ICollection<ReservationService> ReservationServices { get; set; }

	}
}
