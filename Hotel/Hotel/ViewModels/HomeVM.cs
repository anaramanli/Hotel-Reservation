namespace Hotel.ViewModels;

public class HomeVM
{
    public List<Models.Slider> Sliders { get; set; }
    public List<Models.AboutCompany> AboutCompanies { get; set; }
    public List<Models.Room> Rooms { get; set; }
    public List<Models.Feature> Features { get; set; }
    public List<Models.AppUser>? AppUsers { get; set; }
    public List<Models.Reservation>? Reservations { get; set; }
    public List<Models.UserReservation>? UserReservations { get; set; }

}
