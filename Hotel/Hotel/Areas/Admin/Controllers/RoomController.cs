using Hotel.DAL;
using Hotel.Models;
using Hotel.ViewModels.Room;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Hotel.Extensions;

namespace Hotel.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class RoomController : Controller
    {
        private readonly HotelDBContext _context;
        private readonly IWebHostEnvironment _env;

        public RoomController(HotelDBContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        // GET: RoomController
        public async Task<IActionResult> Index(int page = 0)
        {
            int pageSize = 4;
            var totalItems = await _context.Rooms.CountAsync();
            ViewBag.MaxPage = (int)Math.Ceiling((double)totalItems / pageSize);
            ViewBag.CurrentPage = page;

            var rooms = await _context.Rooms
                .Include(r => r.Category)
                .Include(r => r.RoomStatus)
                .Skip(page * pageSize)
                .Take(pageSize)
                .Select(r => new GetRoomAdminVM
                {
                    Id = r.Id,
                    RoomNumber = r.RoomNumber,
                    Name = r.Name,
                    Description = r.Description,
                    Price = r.Price,
                    ImageUrl = r.ImageUrl,
                    CheckIn = r.CheckIn,
                    CategoryId = r.Category.Id,
                    CategoryName = r.Category.CategoryName,
                    RoomStatusId = r.RoomStatus.Id,
                    RoomStatusName = r.RoomStatus.StatusName
                }).ToListAsync();

            return View(rooms);
        }

        // GET: RoomController/Create
        public async Task<IActionResult> Create()
        {
            var categories = await _context.Categories.ToListAsync();
            var roomStatuses = await _context.RoomStatuses.ToListAsync();

            var viewModel = new CreateRoomAdminVM
            {
                Categories = categories,
                RoomStatuses = roomStatuses
            };
            return View(viewModel);
        }

        // POST: RoomController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateRoomAdminVM vm)
        {
            if (ModelState.IsValid)
            {
                if (vm.ImageFile != null)
                {
                    if (!vm.ImageFile.IsValidType("image"))
                        ModelState.AddModelError("ImageFile", "File must be an image.");
                    if (!vm.ImageFile.IsValidLength(2000))
                        ModelState.AddModelError("ImageFile", "File size must be lower than 3mb.");

                    if (ModelState.IsValid)
                    {
                        string fileName = await vm.ImageFile.SaveFileAsync(Path.Combine(_env.WebRootPath, "assets", "imgs"));

                        Room room = new Room
                        {
                            RoomNumber = vm.RoomNumber,
                            Name = vm.Name,
                            Description = vm.Description,
                            Price = vm.Price,
                            ImageUrl = fileName,
                            CheckIn = vm.CheckIn,
                            CategoryId = vm.CategoryId,
                            RoomStatusId = vm.RoomStatusId
                        };

                        _context.Add(room);
                        await _context.SaveChangesAsync();

                        return RedirectToAction(nameof(Index));
                    }
                }
            }

            vm.Categories = await _context.Categories.ToListAsync();
            vm.RoomStatuses = await _context.RoomStatuses.ToListAsync();
            TempData["CreateStatus"] = "failure";
            return View(vm);
        }

        // GET: RoomController/Edit/5
        public IActionResult Edit(int id)
        {
            return View();
        }

        // POST: RoomController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, IFormCollection collection)
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

        // GET: RoomController/Delete/5
        public IActionResult Delete(int id)
        {
            return View();
        }

        // POST: RoomController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id, IFormCollection collection)
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
    }
}
