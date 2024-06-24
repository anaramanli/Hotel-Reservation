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
                                .Include(r=> r.Category)
                                .FirstOrDefault(r => r.Id == id);
            if (room == null || !room.RoomStatus.StatusName.ToString().Contains("Available"))
            {
                return BadRequest();
            }
            var viewModel = new ReservationVM
            {
                Room = room,
                RoomId = room.Id,
                Price = room.Price, // Set the Price property
                CheckInDate = DateTime.Now,
                CheckOutDate = DateTime.Now.AddDays(1), // Default to 1 night stay
                Name = User.Identity.Name,
                TotalCost = room.Price
            };

            ViewBag.Extras = Enum.GetValues(typeof(Extras)).Cast<Extras>().ToList();
            ViewBag.ExtrasPrices = ExtrasPrices;

            return View(viewModel);
        }

        [HttpPost]
        [Authorize]
        public IActionResult Create(ReservationVM viewModel)
        {
            var room = _context.Rooms.Include(r => r.RoomStatus).FirstOrDefault(r => r.Id == viewModel.RoomId);
            viewModel.Price = room.Price;

            if (ModelState.IsValid)
            {
                if (room != null)
                {
                    // Calculate total cost including extras
                    viewModel.CalculateTotalCost(ExtrasPrices);

                    var reservation = new Reservation
                    {
                        Room = room,
                        Name = viewModel.Name,
                        Surname = viewModel.Surname,
                        PhoneNumber = viewModel.PhoneNumber,
                        Email = viewModel.Email,
                        CheckInDate = viewModel.CheckInDate,
                        CheckOutDate = viewModel.CheckOutDate,
                        Message = viewModel.Message,
                        SelectedExtras = viewModel.SelectedExtras,
                        TotalCost = viewModel.TotalCost,
                        
                    };

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
            return View(viewModel);
        }


    }
}
