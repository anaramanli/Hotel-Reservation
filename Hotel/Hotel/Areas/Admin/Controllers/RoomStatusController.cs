using Hotel.DAL;
using Hotel.Models;
using Hotel.ViewModels.RoomStatus;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Hotel.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class RoomStatusController(HotelDBContext _context) : Controller
    {
        // GET: RoomStatusController
        public async Task<IActionResult> Index(int page = 0)
        {
            int pageSize = 5;
            var totalItems = await _context.RoomStatuses.CountAsync();
            ViewBag.MaxPage = (int)Math.Ceiling((double)totalItems / pageSize);
            ViewBag.CurrentPage = page;
            var data = await _context.RoomStatuses.Skip(page * pageSize).Take(pageSize)
                .Select(r=> new GetRoomStatusAdminVM{
                Id = r.Id,
                StatusName = r.StatusName,
            }).ToListAsync();
            return View(data);
        }

        // GET: RoomStatusController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: RoomStatusController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateRoomStatusAdminVM vm)
        {
            if (ModelState.IsValid)
            {
                if (!await _context.RoomStatuses.AnyAsync(c => c.StatusName == vm.StatusName))
                {
                    RoomStatus roomStatus = new RoomStatus
                    {
                        StatusName = vm.StatusName,
                        IsDeleted = false,
                        CreatedAt = DateTime.Now,
                    };

                    await _context.AddAsync(roomStatus);
                    await _context.SaveChangesAsync();

                    TempData["CreateStatus"] = "success";
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    TempData["CreateStatus"] = "exists";
                    return View(vm);
                }
            }

            TempData["CreateStatus"] = "failure";
            return View(vm);
        }

        // GET: RoomStatusController/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            return View();
        }

        // POST: RoomStatusController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: RoomStatusController/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            if (id == null || id < 1) return BadRequest();

            var data = await _context.RoomStatuses.FirstOrDefaultAsync(r => r.Id == id);

            if (data == null) return NotFound();

            _context.Remove(data);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

    }
}
