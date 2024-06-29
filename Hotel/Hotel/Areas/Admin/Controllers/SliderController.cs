using Hotel.DAL;
using Hotel.ViewModels.Slider;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Text;
using Hotel.Extensions;
using Hotel.Models;
using Microsoft.AspNetCore.Authorization;

namespace Hotel.Areas.Admin.Controllers
{
    [Area("Admin")]
	[Authorize(Roles ="Admin")]
    public class SliderController(HotelDBContext _context, IWebHostEnvironment _env) : Controller
    {
		// GET: SliderController
		public async Task<ActionResult> Index(int page = 0)
		{
			int pageSize = 4;
			var totalItems = await _context.Sliders.CountAsync();
			ViewBag.MaxPage = (int)Math.Ceiling((double)totalItems / pageSize);
			ViewBag.CurrentPage = page;

			var data = await _context.Sliders.Skip(page * pageSize).Take(pageSize)
				.Select(s => new GetSliderAdminVM
				{
					Id = s.Id,
					ImageUrl = s.ImageUrl,
					Title = s.Title
				}).ToListAsync();

			return View(data);
		}

		// GET: SliderController/Create
		public ActionResult Create()
        {
            return View();
        }

        // POST: SliderController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(CreateSliderAdminVM vm)
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
                        Slider slider = new Slider
                        {
                            CreatedAt = DateTime.Now,
                            ImageUrl = fileName,
                            Title = vm.Title
                        };
                        await _context.AddAsync(slider);
                        await _context.SaveChangesAsync();
                        return RedirectToAction(nameof(Index));
                    }
                }
                else
                {
                    ModelState.AddModelError("ImageFile", "Please choose a file to upload.");
                }
            }
            return View(vm);
        }

        // GET: SliderController/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || id < 1)
                return BadRequest();

            var slider = await _context.Sliders.FirstOrDefaultAsync(s => s.Id == id);
            if (slider == null)
                return NotFound();

            var vm = new UpdateSliderAdminVM
            {
                Id = slider.Id,
                Title = slider.Title,
                ImageUrl = slider.ImageUrl
            };
            return View(vm);
        }

        // POST: SliderController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, UpdateSliderAdminVM vm)
        {
            if (id != vm.Id)
                return BadRequest();

            if (ModelState.IsValid)
            {
                var slider = await _context.Sliders.FirstOrDefaultAsync(s => s.Id == id);
                if (slider == null)
                    return NotFound();

                if (vm.ImageFile != null)
                {
                    if (!vm.ImageFile.IsValidType("image"))
                    {
                        ModelState.AddModelError("ImageFile", "File must be an image.");
                        return View(vm);
                    }
                    if (!vm.ImageFile.IsValidLength(2000))
                    {
                        ModelState.AddModelError("ImageFile", "File size must be lower than 3MB.");
                        return View(vm);
                    }

                    string newFileName = await vm.ImageFile.SaveFileAsync(Path.Combine(_env.WebRootPath, "assets", "imgs"));
                    if (!string.IsNullOrEmpty(slider.ImageUrl))
                    {
                        var oldFilePath = Path.Combine(_env.WebRootPath, "assets", "imgs", slider.ImageUrl);
                        if (System.IO.File.Exists(oldFilePath))
                        {
                            System.IO.File.Delete(oldFilePath);
                        }
                    }
                    slider.ImageUrl = newFileName;
                }

                slider.Title = vm.Title;

                _context.Update(slider);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(vm);
        }

        // GET: SliderController/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || id < 1)
                return BadRequest();

            var slider = await _context.Sliders.FirstOrDefaultAsync(s => s.Id == id);
            if (slider == null)
                return NotFound();

            _context.Sliders.Remove(slider);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        public ActionResult Chat()
        {
            return View();
        }
    }
}
