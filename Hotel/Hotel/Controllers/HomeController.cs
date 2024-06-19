using Hotel.DAL;
using Hotel.Models;
using Hotel.Services;
using Hotel.ViewModels;
using Hotel.ViewModels.RoomDetail;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Hotel.Controllers
{
    public class HomeController(HotelDBContext _context) : Controller
    {
        public async Task<IActionResult> Index()
        {
            //await _emailService.SendeMailAsync();
            List<Slider> sliders = await _context.Sliders
                .Where(s=> !s.IsDeleted).ToListAsync();
			List<AboutCompany> companies = await _context.AboutCompanies
				.Where(ac => !ac.IsDeleted).ToListAsync();
			List<Room> rooms = await _context.Rooms.Include(r=> r.Images).Include(r=> r.Category)
                .Where(ac => !ac.IsDeleted).ToListAsync(); 
            List<Feature> features = await _context.Features
				.Where(ac => !ac.IsDeleted).ToListAsync();
			HomeVM homeVM = new HomeVM
            {
                Sliders = sliders,
                AboutCompanies = companies,
                Rooms = rooms,
                Features = features
            };
            return View(homeVM);  
        }
        public async Task<IActionResult> Details(int id)
        {
            var room = await _context.Rooms
                .Include(r => r.Images)
                .Include(r => r.Category)
                .Include(r => r.RoomStatus)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (room == null)
            {
                return NotFound();
            }

            var availabilities = await _context.Availabilities
                .Where(a => a.RoomId == id)
                .ToListAsync();

            var viewModel = new RoomDetailsViewModel
            {
                Room = room,
                Availabilities = availabilities
            };

            return View(viewModel);
        }

    }
}
