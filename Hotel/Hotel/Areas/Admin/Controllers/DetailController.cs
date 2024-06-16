using Hotel.DAL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Hotel.Areas.Admin.Controllers
{
    public class DetailController(HotelDBContext _context) : Controller
    {
        // GET: DetailController
        public async Task<IActionResult> Index(int page = 0)
        {
            int pageSize = 4;
            var totalItems = await _context.Rooms.CountAsync();
            ViewBag.MaxPage = (int)Math.Ceiling((double)totalItems / pageSize);
            ViewBag.CurrentPage = page;
            return View();
        }

        // GET: DetailController/Create
        public async Task<IActionResult> Create()
        {
            return View();
        }

        // POST: DetailController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(IFormCollection collection)
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

        // GET: DetailController/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            return View();
        }

        // POST: DetailController/Edit/5
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

        // GET: DetailController/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            return View();
        }
    }
}
