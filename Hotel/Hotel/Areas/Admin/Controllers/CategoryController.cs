using Hotel.DAL;
using Hotel.Models;
using Hotel.ViewModels.Category;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Hotel.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController(HotelDBContext _context, IWebHostEnvironment _env) : Controller
    {
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
        public ActionResult Create()
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
                if (!_context.Categories.Any(c => c.CategoryName == vm.CategoryName))
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
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: CategoryController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
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
