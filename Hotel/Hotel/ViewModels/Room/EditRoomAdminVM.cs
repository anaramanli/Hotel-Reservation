using System.ComponentModel.DataAnnotations;

namespace Hotel.ViewModels.Room
{
	public class EditRoomAdminVM
	{
        public int Id { get; set; }

        [Required(ErrorMessage = "Room number is required.")]
        public string RoomNumber { get; set; }

        [Required(ErrorMessage = "Room name is required.")]
        public string Name { get; set; }

        public string Description { get; set; }

        [Required(ErrorMessage = "Price is required.")]
        [DataType(DataType.Currency)]
        public decimal Price { get; set; }
        [Required(ErrorMessage = "Location is required.")]

        public string Location { get; set; }
        [Required(ErrorMessage = "Rating is required.")]
		[Range(1, 10, ErrorMessage = "Number of guests must be between 1 and 10.")]
		public int Rating { get; set; }

        [Required(ErrorMessage = "Number of beds is required.")]
        public int Beds { get; set; }

        [Required(ErrorMessage = "Number of bathrooms is required.")]
        public int Bathrooms { get; set; }

        public List<string> ImageUrls { get; set; } = new List<string>();

        [Required(ErrorMessage = "Please select at least one image.")]
        public List<IFormFile> ImageFiles { get; set; } = new List<IFormFile>();

        [Required(ErrorMessage = "Check-in time is required.")]
        public TimeOnly CheckIn { get; set; } = new TimeOnly(6, 0);

        [Required(ErrorMessage = "Category is required.")]
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "Room status is required.")]
        public int RoomStatusId { get; set; }

        public IEnumerable<Models.Category>? Categories { get; set; }
        public IEnumerable<Models.RoomStatus>? RoomStatuses { get; set; }
    }
}
