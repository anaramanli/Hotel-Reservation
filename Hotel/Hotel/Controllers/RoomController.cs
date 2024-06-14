using Hotel.DAL;
using Hotel.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Hotel.Controllers
{
	public class RoomController(HotelDBContext _context) : Controller
	{
		public async Task<IActionResult> Index()
		{
			List<Room> rooms = await _context.Rooms.Include(r => r.Images).Include(r => r.Category)
				.Where(ac => !ac.IsDeleted).ToListAsync();
			return View(rooms);
		}
	}
}
