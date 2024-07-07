using Hotel.DAL;
using Hotel.Models;
using Hotel.ViewModels;
using Hotel.ViewModels.UserProfile;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hotel.Controllers
{
	[Authorize]
	public class UserProfileController : Controller
	{
		private readonly HotelDBContext _context;
		private readonly UserManager<AppUser> _usermanager;

		public UserProfileController(HotelDBContext context, UserManager<AppUser> userManager)
		{
			_context = context;
			_usermanager = userManager;
		}

		public async Task<IActionResult> Index()
		{
			var user = await _usermanager.GetUserAsync(User);
			

			var reservations = await _context.Reservations.Where(r=>r.IsDeleted != true)
				.Include(r => r.Room).ThenInclude(ri=>ri.Images)
				.Where(r => r.Customer.AppUserId == user.Id)
				.ToListAsync();
			var viewModel = new UserProfileViewModel
			{
				UserName = user.Name,
				UserSurname = user.Surname,
				UserEmail = user.Email,
				Reservations = reservations,
			};

			return View(viewModel);
		}
	}
}
