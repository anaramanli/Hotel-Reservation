using Hotel.DAL;
using Hotel.Models;
using Hotel.ViewModels.Category;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Hotel.Areas.Admin.Controllers
{
    [Area("Admin")]
	[Authorize(Roles ="Admin")]
    public class CategoryController : Controller
    {
        private readonly HotelDBContext _context;
        private readonly IWebHostEnvironment _env;

        public CategoryController(HotelDBContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        // GET: CategoryController
        public async Task<IActionResult> Index(int page = 0)
        {
            int pageSize = 5;
            var totalItems = await _context.Categories.CountAsync();
            ViewBag.MaxPage = (int)Math.Ceiling((double)totalItems / pageSize);
            ViewBag.CurrentPage = page;
            var data = await _context.Categories.Skip(page * pageSize).Take(pageSize)
                .Select(c => new GetCategoryAdminVM
                {
                    Id = c.Id,
                    CategoryName = c.CategoryName,
                }).ToListAsync();
            return View(data);
        }

        // GET: CategoryController/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: CategoryController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateCategoryAdminVM vm)
        {
            if (ModelState.IsValid)
            {
                if (!await _context.Categories.AnyAsync(c => c.CategoryName == vm.CategoryName))
                {
                    Category category = new Category
                    {
                        CategoryName = vm.CategoryName,
                        CreatedAt = DateTime.Now,
                    };

                    await _context.AddAsync(category);
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

        // GET: CategoryController/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return BadRequest();

            var category = await _context.Categories.FirstOrDefaultAsync(c => c.Id == id);

            if (category == null) return NotFound();

            var vm = new EditCategoryAdminVM
            {
                Id = category.Id,
                CategoryName = category.CategoryName,
            };
            return View(vm);
        }

        // POST: CategoryController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EditCategoryAdminVM vm)
        {
            if (id != vm.Id)
                return BadRequest();
            if (ModelState.IsValid)
            {
                var category = await _context.Categories.FirstOrDefaultAsync(c => c.Id == id);

                if (category == null) return NotFound();
                category.CategoryName = vm.CategoryName;
                category.ModifiedAt = DateTime.Now;
                _context.Update(category);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(vm);
        }

        // GET: CategoryController/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || id < 1) return BadRequest();

            var data = await _context.Categories.FirstOrDefaultAsync(c => c.Id == id);

            if (data == null) return NotFound();

            _context.Remove(data);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
