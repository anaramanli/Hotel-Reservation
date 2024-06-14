using Hotel.Models;
using System.ComponentModel.DataAnnotations;
namespace Hotel.ViewModels.Room
{
    public class CreateRoomAdminVM
    {
        [Required]
        public string RoomNumber { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        public decimal Price { get; set; }
        [Required]
        public int Rating { get; set; }
        [Required]
        public int Beds { get; set; }
        [Required]
        public int Bathrooms { get; set; }

        [Required]
        //public string ImageUrl { get; set; }
        public List<IFormFile> ImageFiles { get; set; }
        [Required]
        public TimeOnly CheckIn { get; set; } = new TimeOnly(6, 00);

        // Relation
        [Required]
        public int CategoryId { get; set; }

        [Required]
        public int RoomStatusId { get; set; }

        public IEnumerable<Models.Category>? Categories { get; set; }
        public IEnumerable<Models.RoomStatus>? RoomStatuses { get; set; }
    }
}
