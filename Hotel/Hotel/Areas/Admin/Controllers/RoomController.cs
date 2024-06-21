using Hotel.DAL;

using Hotel.ViewModels.Room;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Hotel.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Hotel.Models;
using Microsoft.AspNetCore.Authorization;

namespace Hotel.Areas.Admin.Controllers
{
    [Area("Admin")]
	[Authorize(Roles ="Admin")]
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
                .Include(r => r.Images)
                .Skip(page * pageSize)
                .Take(pageSize)
                .Select(r => new GetRoomAdminVM
                {
                    Id = r.Id,
                    RoomNumber = r.RoomNumber,
                    Name = r.Name,
                    Description = r.Description,
                    Price = r.Price,
                    Bathrooms = r.Bathrooms,
                    Beds = r.Beds,
                    Rating = r.Rating,
                    CheckIn = r.CheckIn,
                    CategoryId = r.Category.Id,
                    CategoryName = r.Category.CategoryName,
                    RoomStatusId = r.RoomStatus.Id,
                    RoomStatusName = r.RoomStatus.StatusName,
                    ImageUrls = r.Images.Select(img => img.Url).ToList()
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
                var room = new Room
                {
                    RoomNumber = vm.RoomNumber,
                    Name = vm.Name,
                    Description = vm.Description,
                    Price = vm.Price,
                    Bathrooms = vm.Bathrooms,
                    Beds = vm.Beds,
                    Rating = vm.Rating,
                    CheckIn = vm.CheckIn,
                    CategoryId = vm.CategoryId,
                    RoomStatusId = vm.RoomStatusId
                };

                if (vm.ImageFiles != null && vm.ImageFiles.Count > 0)
                {
                    foreach (var imageFile in vm.ImageFiles)
                    {
                        if (!imageFile.IsValidType("image"))
                            ModelState.AddModelError("ImageFiles", "Dosya bir resim olmalıdır.");
                        if (!imageFile.IsValidLength(3000))
                            ModelState.AddModelError("ImageFiles", "Dosya boyutu 3MB'den küçük olmalıdır.");

                        if (ModelState.IsValid)
                        {
                            string fileName = await imageFile.SaveFileAsync(Path.Combine(_env.WebRootPath, "assets", "imgs"));
                            room.Images.Add(new RoomImage { Url = fileName });
                        }
                    }
                }

                if (ModelState.IsValid)
                {
                    _context.Add(room);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }

            vm.Categories = await _context.Categories.ToListAsync();
            vm.RoomStatuses = await _context.RoomStatuses.ToListAsync();
            TempData["CreateStatus"] = "failure";
            return View(vm);
        }

        // GET: RoomController/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || id < 1)
                return BadRequest();

            var room = await _context.Rooms
               .Include(r => r.Images)
               .Include(r => r.Category)
               .Include(r => r.RoomStatus)
               .FirstOrDefaultAsync(r => r.Id == id);
            if (room == null)
            {
                return NotFound();
            }
            var viewModel = new EditRoomAdminVM
            {
                Id = room.Id,
                RoomNumber = room.RoomNumber,
                Name = room.Name,
                Description = room.Description,
                Price = room.Price,
                Rating = room.Rating,
                Beds = room.Beds,
                Bathrooms = room.Bathrooms,
                CheckIn = room.CheckIn,
                CategoryId = room.CategoryId,
                RoomStatusId = room.RoomStatusId,
                ImageUrls = room.Images.Select(i => i.Url).ToList(),
                Categories = await _context.Categories.ToListAsync(),
                RoomStatuses = await _context.RoomStatuses.ToListAsync()
            };
            return View(viewModel);
        }

        // POST: RoomController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(int id, EditRoomAdminVM vm)
		{
			if (id != vm.Id)
			{
				return BadRequest();
			}

			if (ModelState.IsValid)
			{
				try
				{
					var room = await _context.Rooms
						.Include(r => r.Images)
						.FirstOrDefaultAsync(r => r.Id == id);

					if (room == null)
					{
						return NotFound();
					}

					room.RoomNumber = vm.RoomNumber;
					room.Name = vm.Name;
					room.Description = vm.Description;
					room.Price = vm.Price;
					room.Bathrooms = vm.Bathrooms;
					room.Beds = vm.Beds;
					room.Rating = vm.Rating;
					room.CheckIn = vm.CheckIn;
					room.CategoryId = vm.CategoryId;
					room.RoomStatusId = vm.RoomStatusId;

					if (vm.ImageFiles != null && vm.ImageFiles.Count > 0)
					{
						room.Images.Clear();

						foreach (var imageFile in vm.ImageFiles)
						{
							if (!imageFile.IsValidType("image"))
								ModelState.AddModelError("ImageFiles", "Please choose a file to upload.");
							if (!imageFile.IsValidLength(3000))
								ModelState.AddModelError("ImageFiles", "File size must be lower than 3MB.");

							if (ModelState.IsValid)
							{
								string fileName = await imageFile.SaveFileAsync(Path.Combine(_env.WebRootPath, "assets", "imgs"));
								room.Images.Add(new RoomImage { Url = fileName });
							}
						}
					}

					_context.Update(room);
					await _context.SaveChangesAsync();
					TempData["EditStatus"] = "success";
					return RedirectToAction(nameof(Index));
				}
				catch (DbUpdateConcurrencyException)
				{
					if (!await RoomExists(id))
					{
						return NotFound();
					}
					else
					{
						throw;
					}
				}
			}

			vm.Categories = await _context.Categories.ToListAsync();
			vm.RoomStatuses = await _context.RoomStatuses.ToListAsync();
			TempData["EditStatus"] = "failure";
			return View(vm);
		}

		// GET: RoomController/Delete/5
		public async Task<IActionResult> Delete(int id)
        {
            if (id == null || id < 1) return BadRequest();

            var data = await _context.Rooms.FirstOrDefaultAsync(r => r.Id == id);

            if (data == null) return NotFound();

            _context.Remove(data);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> RoomExists(int id)
        {
            return await _context.Rooms.AnyAsync(r => r.Id == id);
        }
    }
}
