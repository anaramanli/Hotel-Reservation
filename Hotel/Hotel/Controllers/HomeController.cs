using Hotel.DAL;
using Hotel.Models;
using Hotel.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Hotel.Controllers
{
    public class HomeController(HotelDBContext _context) : Controller
    {
        public async Task<IActionResult> Index()
        {
            List<Slider> sliders = await _context.Sliders
                .Where(s=> !s.IsDeleted).ToListAsync();
            List<AboutCompany> companies = await _context.AboutCompanies
                .Where(ac => !ac.IsDeleted).ToListAsync(); 
            List<Room> rooms = await _context.Rooms.Include(r=> r.Images).Include(r=> r.Category)
                .Where(ac => !ac.IsDeleted).ToListAsync();
            HomeVM homeVM = new HomeVM
            {
                Sliders = sliders,
                AboutCompanies = companies,
                Rooms = rooms
            };
            return View(homeVM);  
        }
    }
}
