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
using Stripe;
using System.Threading.Tasks;
using Hotel.Interfaces;
using Hotel.Services;
using System.Net.Mail;
using System.Net.Mime;

namespace Hotel.Controllers
{
    public class OrderTableController : Controller
    {
        private readonly HotelDBContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly IEmailService _emailService;
        private readonly IQRCodeService _qrCodeService;

        private static readonly Dictionary<Extras, decimal> ExtrasPrices = new Dictionary<Extras, decimal>
    {
        { Extras.NightView, 50 },
        { Extras.OceanView, 100 },
        { Extras.CityView, 0 }
    };

        public OrderTableController(HotelDBContext context, UserManager<AppUser> userManager, IEmailService emailService, IQRCodeService qrCodeService)
        {
            _context = context;
            _userManager = userManager;
            _emailService = emailService;
            _qrCodeService = qrCodeService;
        }

        [Authorize]
        public async Task<IActionResult> Create(int id)
        {
            var room = await _context.Rooms
                                    .Include(r => r.RoomStatus)
                                    .Include(r => r.Category)
                                    .FirstOrDefaultAsync(r => r.Id == id);
            if (room == null || !room.RoomStatus.StatusName.Contains("Available"))
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
                Name = user.UserName,
                Email = user.Email,
            };

            ViewBag.Extras = Enum.GetValues(typeof(Extras)).Cast<Extras>().ToList();
            ViewBag.ExtrasPrices = ExtrasPrices;

            return View(viewModel);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create(ReservationVM viewModel, string stripeEmail, string stripeToken)
        {
            var room = await _context.Rooms.Include(r => r.RoomStatus).FirstOrDefaultAsync(r => r.Id == viewModel.RoomId);
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
                        customer = new Models.Customer
                        {
                            AppUserId = user.Id,
                            AppUser = user,
                            CreatedAt = DateTime.Now,
                            FirstName = user.Name,
                            LastName = user.Surname,
                            Email = user.Email,
                            Phone = user.PhoneNumber,
                        };
                        _context.Customers.Add(customer);
                        await _context.SaveChangesAsync();
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
                        CustomerId = customer.Id,
                        CreatedAt = DateTime.Now
                    };

                    // Calculate the total cost
                    reservation.CalculateTotalCost(ExtrasPrices);

                    var reservedStatus = await _context.RoomStatuses.FirstOrDefaultAsync(rs => rs.StatusName == "Reserved");
                    if (reservedStatus == null)
                    {
                        return BadRequest("Reserved status not found.");
                    }

                    // Stripe
                    var optionCust = new CustomerCreateOptions
                    {
                        Email = stripeEmail,
                        Name = user.Name + " " + user.Surname,
                        Phone = user.PhoneNumber
                    };
                    var serviceCust = new CustomerService();
                    var stripeCustomer = serviceCust.Create(optionCust);

                    var totalCostInCents = (long)(reservation.TotalCost * 100);
                    var optionsCharge = new ChargeCreateOptions
                    {
                        Amount = totalCostInCents,
                        Currency = "USD",
                        Description = "Reservation payment",
                        Source = stripeToken,
                        ReceiptEmail = stripeEmail
                    };
                    var serviceCharge = new ChargeService();
                    var charge = serviceCharge.Create(optionsCharge);

                    if (charge.Status != "succeeded")
                    {
                        ModelState.AddModelError("Address", "Payment failed.");
                        return View(viewModel);
                    }

                    room.RoomStatus = reservedStatus;

                    var qrCodeText = $"Room ID: {reservation.RoomId}, Check-in: {reservation.CheckInDate}, Check-out: {reservation.CheckOutDate}";
                    var qrCodeBytes = _qrCodeService.GenerateQRCodeAsByteArray(qrCodeText);

                    string body = $@"
                    <h1>Payment Successful</h1>
                    <p>Thank you for your reservation. Your payment has been processed successfully.</p>
                    <p>Reservation Details:</p>
                    <ul>
                        <li>Check-in Date: {reservation.CheckInDate.ToShortDateString()}</li>
                        <li>Check-out Date: {reservation.CheckOutDate.ToShortDateString()}</li>
                        <li>Total Cost: ${reservation.TotalCost}</li>
                    </ul>
                    <p>Scan this QR code for room access:</p>
                    <img src='cid:QRCodeImage' alt='QR Code' width='40%' />
                ";


                    await _emailService.SendMailWithEmbeddedImageAsync(user.Email, "Reservation Confirmation", body, qrCodeBytes, "QRCodeImage", true);

                    _context.Reservations.Add(reservation);
                    _context.Rooms.Update(room);
                    await _context.SaveChangesAsync();

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