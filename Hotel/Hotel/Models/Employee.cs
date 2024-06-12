using Hotel.Models.Base;

namespace Hotel.Models
{
	public class Employee : BaseEntity
	{
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string Position { get; set; }
		public string Email { get; set; }
		public string Phone { get; set; }
	}
}
