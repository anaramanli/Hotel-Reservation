using Hotel.DAL;
using Hotel.Models;
using Hotel.ViewModels.UserProfile;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Hotel.Areas.Admin.Controllers
{
	[Area("Admin")] 
	public class UserController(HotelDBContext _context, UserManager<AppUser> _userManager) : Controller
	{
        // GET: UserController
        public async Task<IActionResult> Index()
        {
			//        var user = await _userManager.GetUserAsync(User);

			//        var reservations = await _context.Reservations
			//            .Include(r => r.Room)
			//.ThenInclude(ri => ri.Images)
			//            .Where(r => r.Customer.AppUserId == user.Id)
			//            .ToListAsync();

			//        var users = await _context.AppUsers
			//.Include(u=>u.UserReservations)
			//.ThenInclude(r=>r.Reservation)
			//.ToListAsync();

			//        var viewModel = new UserProfileViewModel
			//        {
			//            UserName = $"{user.Name} {user.Surname}",
			//            UserSurname = user.Surname,
			//            UserEmail = user.Email,
			//            Reservations = reservations,
			//            AppUsers = users
			//        };

			//        return View(viewModel);
			var users = await _context.AppUsers.ToListAsync();

			var reservations = await _context.Reservations
				.Include(r => r.Room).ThenInclude(ri => ri.Images)
				.Where(r => users.Select(u => u.Id).Contains(r.Customer.AppUserId))
				.ToListAsync();

			var viewModel = new UserProfileViewModel
			{
				AppUsers = users,
				Reservations = reservations
			};

			return View(viewModel);
		}

        // GET: UserController/Details/5
        public ActionResult Details(int id)
		{
			return View();
		}
	}
}
