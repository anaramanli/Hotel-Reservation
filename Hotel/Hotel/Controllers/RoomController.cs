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

        public async Task<IActionResult> Index(int page = 0)
        {
            int pageSize = 5;
            var totalItems = await _context.Rooms.CountAsync();
            ViewBag.MaxPage = (int)Math.Ceiling((double)totalItems / pageSize);
            ViewBag.CurrentPage = page;

            List<Slider> sliders = await _context.Sliders.Where(s => !s.IsDeleted).ToListAsync();
            List<AboutCompany> companies = await _context.AboutCompanies.Where(ac => !ac.IsDeleted).ToListAsync();
            List<Room> rooms = await _context.Rooms.Skip(page * pageSize).Take(pageSize)
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
                .Include(r=>r.Comments)/*.Where(c => !c.IsDeleted))*/
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
        public async Task<IActionResult> Search(string query = "", int page = 0)
        {
            int pageSize = 5;
            var roomsQuery = _context.Rooms
                .Include(r => r.Images)
                .Include(r => r.Category)
                .Include(r => r.RoomStatus)
                .Where(r => !r.IsDeleted);

            if (!string.IsNullOrEmpty(query))
            {
                roomsQuery = roomsQuery
                    .Where(r => r.Name.Contains(query) ||
                                r.Description.Contains(query) ||
                                r.RoomStatus.StatusName.Contains(query) ||
                                r.Rating.ToString().Contains(query) ||
                                (r.Category != null && r.Category.CategoryName.Contains(query)));
            }

            var totalItems = await roomsQuery.CountAsync();
            var rooms = await roomsQuery
                .Skip(page * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var result = new
            {
                rooms = rooms.Select(r => new
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
                }).ToList(),
                currentPage = page,
                totalPages = (int)Math.Ceiling((double)totalItems / pageSize)
            };

            return Json(result);
        }

    }
}
