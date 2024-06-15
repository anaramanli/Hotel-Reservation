using Hotel.Models.Base;

namespace Hotel.Models
{
	public class Feature : BaseEntity
	{
        public int ProjectsComplete { get; set; }
        public int LuxuryRooms { get; set; }
        public int Beaches { get; set; }
        public int RegularGuests { get; set; }

        //
        public string FitnessCenter { get; set; }
        public string Jacuzzi { get; set; }
        public string SwimmingPool { get; set; }
        public string SPATreatments { get; set; }
        public string FoodRestaurants { get; set; }
        public string Transportation { get; set; }
	}
}
