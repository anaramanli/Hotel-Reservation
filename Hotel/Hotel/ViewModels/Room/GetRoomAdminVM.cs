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
        public string ImageUrl { get; set; }
        public TimeOnly CheckIn { get; set; } = new TimeOnly(6, 00);
        // Relation
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public int RoomStatusId { get; set; }
        public string RoomStatusName { get; set; }
    }
}
