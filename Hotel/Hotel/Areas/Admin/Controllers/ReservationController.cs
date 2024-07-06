using Hotel.DAL;
using Hotel.Models;
using Hotel.ViewModels.Reservation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Hotel.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/[controller]/[action]")]
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
        [HttpDelete("Delete/{reservationId}")]
        [Authorize]
        public async Task<IActionResult> Delete(int reservationId)
        {
            if (reservationId <= 0)
            {
                return BadRequest("Invalid Reservation ID");
            }

            var reservation = await _context.Reservations
                .Include(r => r.Room)
                .FirstOrDefaultAsync(r => r.Id == reservationId);
            if (reservation == null)
            {
                return NotFound("Reservation not found.");
            }

            var room = reservation.Room;
            var availableStatus = await _context.RoomStatuses.FirstOrDefaultAsync(rs => rs.StatusName == "Available");

            if (availableStatus == null)
            {
                return BadRequest("Available status not found.");
            }

            room.RoomStatus = availableStatus;

            _context.Reservations.Remove(reservation);
            _context.Rooms.Update(room);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Reservation deleted successfully" });
        }

    }
}
