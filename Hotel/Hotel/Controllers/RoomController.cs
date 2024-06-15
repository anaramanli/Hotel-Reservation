using Hotel.DAL;
using Hotel.Models;
using Hotel.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
            List<Room> rooms = await _context.Rooms.Include(r => r.Images).Include(r => r.Category).Where(ac => !ac.IsDeleted).ToListAsync();
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

        [HttpGet]
        public async Task<IActionResult> Search(string query)
        {
            var rooms = await _context.Rooms
                .Include(r => r.Images)
                .Include(r => r.Category)
                .Where(r => !r.IsDeleted &&
                            (r.Name.Contains(query) ||
                             r.Description.Contains(query) ||
                             r.Rating.ToString().Contains(query)))
                .ToListAsync();

            var result = rooms.Select(r => new
            {
                name = r.Name,
                description = r.Description,
                rating = r.Rating,
                beds = r.Beds,
                price = r.Price,
                image = r.Images.Any() ? Url.Content("~/assets/imgs/" + r.Images.First().Url) : Url.Content("~/assets/imgs/default-image.jpg"),
                category = r.Category?.CategoryName
            });

            return Json(result);
        }
    }
}
