using Hotel.DAL;
using Hotel.Models;
using Hotel.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hotel.Controllers
{
    public class RoomController : Controller
    {
        private readonly HotelDBContext _context;

        public RoomController(HotelDBContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            List<Slider> sliders = await _context.Sliders.Where(s => !s.IsDeleted).ToListAsync();
            List<AboutCompany> companies = await _context.AboutCompanies.Where(ac => !ac.IsDeleted).ToListAsync();
            List<Room> rooms = await _context.Rooms
                .Include(r => r.Images)
                .Include(r => r.Category)
                .Include(r => r.RoomStatus).Where(ac => !ac.IsDeleted).ToListAsync();
            List<Feature> features = await _context.Features.Where(ac => !ac.IsDeleted).ToListAsync();

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
                .Include(r=>r.Comments)
                .Include(r => r.Images)
                .Include(r => r.Category)
                .Include(r => r.RoomStatus)
                .FirstOrDefaultAsync(r => r.Id == id && !r.IsDeleted);

            if (room == null)
            {
                return NotFound();
            }

            return View(room);
        }

        [HttpGet]
        public async Task<IActionResult> Search(string query)
        {
            var rooms = await _context.Rooms
                .Include(r => r.Images)
                .Include(r => r.Category)
                .Include(r => r.RoomStatus)
                .Where(r => !r.IsDeleted &&
                            (r.Name.Contains(query) ||
                             r.Description.Contains(query) ||
                             r.RoomStatus.StatusName.Contains(query) ||
                             r.Rating.ToString().Contains(query) ||
                             (r.Category != null && r.Category.CategoryName.Contains(query))))
                .ToListAsync();

            var result = rooms.Select(r => new
            {
                id = r.Id,
                name = r.Name,
                description = r.Description,
                rating = r.Rating,
                beds = r.Beds,
                price = r.Price,
                imageUrl = r.Images.Any() ? r.Images.First().Url : "default-image.jpg",
                categoryName = r.Category?.CategoryName,
                roomstatus = r.RoomStatus.StatusName
            });
            foreach (var item in result)
            {
                await Console.Out.WriteLineAsync(item.roomstatus);
            }
            return Json(result);
        }

    }
}
