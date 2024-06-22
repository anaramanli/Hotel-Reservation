using Hotel.DAL;
using Hotel.Enums;
using Hotel.Models;
using Hotel.ViewModels;
using Hotel.ViewModels.RoomDetail;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Hotel.Controllers
{
    public class OrderTableController : Controller
    {
        private readonly HotelDBContext _context;

        private static readonly Dictionary<Extras, decimal> ExtrasPrices = new Dictionary<Extras, decimal>
        {
            { Extras.NightView, 50 },
            { Extras.OceanView, 100 },
            { Extras.CityView, 75 }
        };

        public OrderTableController(HotelDBContext context)
        {
            _context = context;
        }
        [Authorize]
        public IActionResult Create(int id)
        {

            var room = _context.Rooms
                                .Include(r => r.RoomStatus)
                                .Include(r => r.Images)
                                .FirstOrDefault(r => r.Id == id);
            if (!room.RoomStatus.StatusName.ToString().Contains("Available")) return BadRequest();

            if (room == null)
            {
                return NotFound();
            }

            var viewModel = new Reservation
            {
                Room = room
            };

            ViewBag.Extras = Enum.GetValues(typeof(Extras)).Cast<Extras>().ToList();
            ViewBag.ExtrasPrices = ExtrasPrices;

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult Create(Reservation reservation)
        {
            if (ModelState.IsValid)
            {
                // Retrieve room price from database
                var room = _context.Rooms.FirstOrDefault(r => r.Id == reservation.Room.Id);

                if (room != null)
                {
                    // Calculate total cost including extras
                    reservation.CalculateTotalCost(ExtrasPrices);

                    // Reservation logic (e.g., save to database)
                    _context.Reservations.Add(reservation);
                    _context.SaveChanges();

                    // Redirect to a confirmation page or another action
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Selected room not found.");
                }
            }

            ViewBag.Extras = Enum.GetValues(typeof(Extras)).Cast<Extras>().ToList();
            ViewBag.ExtrasPrices = ExtrasPrices;
            return View(reservation);
        }
    }
}
