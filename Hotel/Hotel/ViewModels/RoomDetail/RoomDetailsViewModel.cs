using Hotel.Models;

namespace Hotel.ViewModels.RoomDetail
{
    public class RoomDetailsViewModel
    {
        public Models.Room Room { get; set; }
        public List<Availability> Availabilities { get; set; }
    }
}
