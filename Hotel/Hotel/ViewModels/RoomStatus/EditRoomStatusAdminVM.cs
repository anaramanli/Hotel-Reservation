using System.ComponentModel.DataAnnotations;

namespace Hotel.ViewModels.RoomStatus
{
    public class EditRoomStatusAdminVM
    {
        public int Id { get; set; }
        [Required]
        public string StatusName { get; set; }
    }
}
