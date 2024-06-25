using Hotel.DAL;
using Hotel.Enums;
using Hotel.Models;
using Hotel.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Collections.Generic;
using Hotel.ViewModels.Reservation;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;

namespace Hotel.Controllers
{
    public class OrderTableController : Controller
    {
        private readonly HotelDBContext _context;
        private readonly UserManager<AppUser> _userManager;

        private static readonly Dictionary<Extras, decimal> ExtrasPrices = new Dictionary<Extras, decimal>
        {
            { Extras.NightView, 50 },
            { Extras.OceanView, 100 },
            { Extras.CityView, 75 }
        };

        public OrderTableController(HotelDBContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [Authorize]
        public async Task<IActionResult> Create(int id)
        {
            var room = _context.Rooms
                                .Include(r => r.RoomStatus)
                                .Include(r => r.Category)
                                .FirstOrDefault(r => r.Id == id);
            if (room == null || !room.RoomStatus.StatusName.ToString().Contains("Available"))
            {
                return BadRequest();
            }

            var user = await _userManager.GetUserAsync(HttpContext.User);
            if (user == null)
            {
                return BadRequest("User not found.");
            }

            var viewModel = new ReservationVM
            {
                Room = room,
                RoomId = room.Id,
                Price = room.Price,
                CheckInDate = DateTime.Now,
                CheckOutDate = DateTime.Now.AddDays(1),
                Name = User.Identity.Name,
                Email = user.Email, 
                TotalCost = room.Price
            };

            ViewBag.Extras = Enum.GetValues(typeof(Extras)).Cast<Extras>().ToList();
            ViewBag.ExtrasPrices = ExtrasPrices;

            return View(viewModel);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create(ReservationVM viewModel)
        {
            var room = _context.Rooms.Include(r => r.RoomStatus).FirstOrDefault(r => r.Id == viewModel.RoomId);
            viewModel.Price = room.Price;

            if (ModelState.IsValid)
            {
                if (room != null)
                {
                    var user = await _userManager.GetUserAsync(User);
                    if (user == null)
                    {
                        return BadRequest("User not found.");
                    }

                    var customer = await _context.Customers.FirstOrDefaultAsync(c => c.AppUserId == user.Id);
                    if (customer == null)
                    {
                        return BadRequest("Customer not found.");
                    }

                    var reservation = new Reservation
                    {
                        Room = room,
                        RoomId = room.Id,
                        Name = viewModel.Name,
                        Surname = viewModel.Surname,
                        PhoneNumber = viewModel.PhoneNumber,
                        Email = user.Email,
                        CheckInDate = viewModel.CheckInDate,
                        CheckOutDate = viewModel.CheckOutDate,
                        Message = viewModel.Message,
                        SelectedExtras = viewModel.SelectedExtras,
                        TotalCost = viewModel.TotalCost,
                        CustomerId = customer.Id 
                    };

                    _context.Reservations.Add(reservation);
                    _context.SaveChanges();

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Selected room not found.");
                }
            }

            ViewBag.Extras = Enum.GetValues(typeof(Extras)).Cast<Extras>().ToList();
            ViewBag.ExtrasPrices = ExtrasPrices;
            return View(viewModel);
        }
    }
}
