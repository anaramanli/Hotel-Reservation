using System.ComponentModel.DataAnnotations;

namespace Hotel.ViewModels.RoomStatus
{
    public class CreateRoomStatusAdminVM
    {
        [Required]
        public string StatusName { get; set; }
    }
}
