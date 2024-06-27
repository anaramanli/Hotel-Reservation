namespace Hotel.ViewModels.UserProfile
{
	public class UserProfileViewModel
	{
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public int ReservationId { get; set; }
		public string UserName { get; set; }
		public string UserSurname { get; set; }
        public string UserEmail { get; set; }
        public List<Models.Reservation> Reservations { get; set; }
		public List<Models.AppUser> AppUsers { get; set; }

	}
}
