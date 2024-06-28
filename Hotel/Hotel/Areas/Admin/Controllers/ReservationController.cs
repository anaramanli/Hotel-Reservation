using Hotel.DAL;
using Hotel.ViewModels.Reservation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Hotel.Areas.Admin.Controllers
{
	[Area("Admin")]
	public class ReservationController : Controller
	{
		private readonly HotelDBContext _context;

		public ReservationController(HotelDBContext context)
		{
			_context = context;
		}

		// GET: /Admin/Reservation/Index
		public async Task<IActionResult> Index()
		{
			var reservations = await _context.Reservations
				.Include(r => r.Room) 
				.ToListAsync();

			var reservationVMs = reservations.Select(r => new ReservationVM
			{
				RoomId = r.RoomId,
				Room = r.Room,
				Name = r.Name,
				Surname = r.Surname,
				PhoneNumber = r.PhoneNumber,
				Email = r.Email,
				CheckInDate = r.CheckInDate,
				CheckOutDate = r.CheckOutDate,
				Message = r.Message,
				SelectedExtras = r.SelectedExtras,
				TotalCost = r.TotalCost
			}).ToList();

			return View(reservationVMs);
		}
	}
}
