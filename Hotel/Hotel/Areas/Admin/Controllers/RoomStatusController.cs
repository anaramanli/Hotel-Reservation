using Hotel.DAL;
using Hotel.Models;
using Hotel.ViewModels.RoomStatus;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Hotel.Areas.Admin.Controllers
{
    [Area("Admin")]
	[Authorize(Roles ="Admin")]
    public class RoomStatusController : Controller
    {
        private readonly HotelDBContext _context;

        public RoomStatusController(HotelDBContext context)
        {
            _context = context;
        }

        // GET: RoomStatusController
        public async Task<IActionResult> Index(int page = 0)
        {
            int pageSize = 5;
            var totalItems = await _context.RoomStatuses.CountAsync();
            ViewBag.MaxPage = (int)Math.Ceiling((double)totalItems / pageSize);
            ViewBag.CurrentPage = page;

            var data = await _context.RoomStatuses
                .OrderBy(r => r.Id)
                .Skip(page * pageSize)
                .Take(pageSize)
                .Select(r => new GetRoomStatusAdminVM
                {
                    Id = r.Id,
                    StatusName = r.StatusName,
                })
                .ToListAsync();

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
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            var status = await _context.RoomStatuses.FirstOrDefaultAsync(c => c.Id == id);

            if (status == null)
            {
                return NotFound();
            }

            var vm = new EditRoomStatusAdminVM
            {
                Id = status.Id,
                StatusName = status.StatusName
            };

            return View(vm);
        }

        // POST: RoomStatusController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EditRoomStatusAdminVM vm)
        {
            if (id != vm.Id)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var status = await _context.RoomStatuses.FindAsync(id);

                    if (status == null)
                    {
                        return NotFound();
                    }

                    status.StatusName = vm.StatusName;
                    status.ModifiedAt = DateTime.Now;

                    _context.Update(status);
                    await _context.SaveChangesAsync();

                    TempData["EditStatus"] = "success";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RoomStatusExists(id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            TempData["EditStatus"] = "failure";
            return View(vm);
        }

        // GET: RoomStatusController/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            if (id < 1)
            {
                return BadRequest();
            }

            var data = await _context.RoomStatuses.FirstOrDefaultAsync(r => r.Id == id);

            if (data == null)
            {
                return NotFound();
            }

            _context.RoomStatuses.Remove(data);
            await _context.SaveChangesAsync();

            TempData["DeleteStatus"] = "success";
            return RedirectToAction(nameof(Index));
        }

        private bool RoomStatusExists(int id)
        {
            return _context.RoomStatuses.Any(e => e.Id == id);
        }
    }
}
