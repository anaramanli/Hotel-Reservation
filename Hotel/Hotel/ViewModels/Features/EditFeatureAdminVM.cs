using System.ComponentModel.DataAnnotations;

namespace Hotel.ViewModels.Features
{
    public class EditFeatureAdminVM
    {
        [Required(ErrorMessage = "The Beaches field is required.")]
        [Range(0, int.MaxValue, ErrorMessage = "Beaches must be a positive number.")]
        public int Beaches { get; set; }

        [Required(ErrorMessage = "The Fitness Center field is required.")]
        public string FitnessCenter { get; set; }

        [Required(ErrorMessage = "The Food Restaurants field is required.")]
        public string FoodRestaurants { get; set; }

        [Required(ErrorMessage = "The Jacuzzi field is required.")]
        public string Jacuzzi { get; set; }

        [Required(ErrorMessage = "The Luxury Rooms field is required.")]
        [Range(0, int.MaxValue, ErrorMessage = "Luxury Rooms must be a positive number.")]
        public int LuxuryRooms { get; set; }

        [Required(ErrorMessage = "The Projects Complete field is required.")]
        [Range(0, int.MaxValue, ErrorMessage = "Projects Complete must be a positive number.")]
        public int ProjectsComplete { get; set; }

        [Required(ErrorMessage = "The Regular Guests field is required.")]
        [Range(0, int.MaxValue, ErrorMessage = "Regular Guests must be a positive number.")]
        public int RegularGuests { get; set; }

        [Required(ErrorMessage = "The SPA Treatments field is required.")]
        public string SPATreatments { get; set; }

        [Required(ErrorMessage = "The Swimming Pool field is required.")]
        public string SwimmingPool { get; set; }

        [Required(ErrorMessage = "The Transportation field is required.")]
        public string Transportation { get; set; }
    }
}
