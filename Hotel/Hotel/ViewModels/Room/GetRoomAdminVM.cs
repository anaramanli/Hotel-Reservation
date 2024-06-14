using Hotel.Models;

namespace Hotel.ViewModels.Room
{
    public class GetRoomAdminVM
    {
        public int Id { get; set; }
        public string RoomNumber { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int Rating { get; set; } = 0;
        public int Beds { get; set; } = 0;
        public int Bathrooms { get; set; } = 0;
        public TimeOnly CheckIn { get; set; } = new TimeOnly(6, 00);

        // Relation
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public int RoomStatusId { get; set; }
        public string RoomStatusName { get; set; }
        public List<string> ImageUrls { get; set; } = new List<string>();
    }
}
