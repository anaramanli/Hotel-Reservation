using Hotel.DAL;
using Hotel.Models;
using Hotel.ViewModels.Reservation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Hotel.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ReservationController : Controller
    {
        private readonly HotelDBContext _context;

        public ReservationController(HotelDBContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var reservations = await _context.Reservations
                .Include(r => r.Room)
                .ToListAsync();

            var vm = reservations.Select(r => new ReservationVM
            {
                Id = r.Id,
                RoomId = r.RoomId,
                Room = r.Room,
                Name = r.Name,
                Surname = r.Surname,
                PhoneNumber = r.PhoneNumber,
                Email = r.Email,
                CheckInDate = r.CheckInDate,
                CheckOutDate = r.CheckOutDate,
                SelectedExtras = r.SelectedExtras,
                TotalCost = r.TotalCost,
                IsDeleted = r.IsDeleted
            }).ToList();

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> ChangeVisibility(int reservationId)
        {
            if (reservationId <= 0)
            {
                return BadRequest("Invalid Reservation ID");
            }

            var reservation = await _context.Reservations
                .FirstOrDefaultAsync(r => r.Id == reservationId);

            if (reservation == null)
            {
                return NotFound();
            }

            reservation.IsDeleted = !reservation.IsDeleted;
            await _context.SaveChangesAsync();

            return Ok();
        }
        [HttpPost]
        public async Task<IActionResult> Delete(int reservationId)
        {
            if (reservationId <= 0)
            {
                return BadRequest("Invalid Reservation ID");
            }

            var reservation = await _context.Reservations.FindAsync(reservationId);
            if (reservation == null)
            {
                return NotFound();
            }

            _context.Reservations.Remove(reservation);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Reservation deleted successfully" });
        }


    }
}
