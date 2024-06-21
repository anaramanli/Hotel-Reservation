using Hotel.DAL;
using Hotel.Models;
using Hotel.ViewModels.Features;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Hotel.Areas.Admin.Controllers
{
    [Area("Admin")]
	[Authorize(Roles ="Admin")]
    public class FeaturesController : Controller
    {
        private readonly HotelDBContext _context;

        public FeaturesController(HotelDBContext context)
        {
            _context = context;
        }

        // GET: FeaturesController
        public async Task<IActionResult> Index(int page = 0)
        {
            int pageSize = 5;
            var totalItems = await _context.Features.CountAsync();
            ViewBag.MaxPage = (int)System.Math.Ceiling((double)totalItems / pageSize);
            ViewBag.CurrentPage = page;

            var data = await _context.Features.Skip(page * pageSize).Take(pageSize)
                .Select(c => new GetFeaturesAdminVM
                {
                    Id = c.Id,
                    Beaches = c.Beaches,
                    FitnessCenter = c.FitnessCenter,
                    FoodRestaurants = c.FoodRestaurants,
                    Jacuzzi = c.Jacuzzi,
                    LuxuryRooms = c.LuxuryRooms,
                    ProjectsComplete = c.ProjectsComplete,
                    RegularGuests = c.RegularGuests,
                    SPATreatments = c.SPATreatments,
                    SwimmingPool = c.SwimmingPool,
                    Transportation = c.Transportation
                }).ToListAsync();
            return View(data);
        }

        // GET: FeaturesController/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: FeaturesController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateFeaturesAdminVM vm)
        {
            if (ModelState.IsValid)
            {
                var feature = new Feature
                {
                    Beaches = vm.Beaches,
                    FitnessCenter = vm.FitnessCenter,
                    FoodRestaurants = vm.FoodRestaurants,
                    Jacuzzi = vm.Jacuzzi,
                    LuxuryRooms = vm.LuxuryRooms,
                    ProjectsComplete = vm.ProjectsComplete,
                    RegularGuests = vm.RegularGuests,
                    SPATreatments = vm.SPATreatments,
                    SwimmingPool = vm.SwimmingPool,
                    Transportation = vm.Transportation
                };

                _context.Features.Add(feature);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(vm);
        }

        // GET: FeaturesController/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var feature = await _context.Features.FindAsync(id);
            if (feature == null)
            {
                return NotFound();
            }

            var vm = new EditFeatureAdminVM
            {
                Beaches = feature.Beaches,
                FitnessCenter = feature.FitnessCenter,
                FoodRestaurants = feature.FoodRestaurants,
                Jacuzzi = feature.Jacuzzi,
                LuxuryRooms = feature.LuxuryRooms,
                ProjectsComplete = feature.ProjectsComplete,
                RegularGuests = feature.RegularGuests,
                SPATreatments = feature.SPATreatments,
                SwimmingPool = feature.SwimmingPool,
                Transportation = feature.Transportation
            };
            return View(vm);
        }

        // POST: FeaturesController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, EditFeatureAdminVM vm)
        {
            if (id == null)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                var feature = await _context.Features.FindAsync(id);
                if (feature == null)
                {
                    return NotFound();
                }

                feature.Beaches = vm.Beaches;
                feature.FitnessCenter = vm.FitnessCenter;
                feature.FoodRestaurants = vm.FoodRestaurants;
                feature.Jacuzzi = vm.Jacuzzi;
                feature.LuxuryRooms = vm.LuxuryRooms;
                feature.ProjectsComplete = vm.ProjectsComplete;
                feature.RegularGuests = vm.RegularGuests;
                feature.SPATreatments = vm.SPATreatments;
                feature.SwimmingPool = vm.SwimmingPool;
                feature.Transportation = vm.Transportation;

                _context.Features.Update(feature);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(vm);
        }


        // POST: FeaturesController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var feature = await _context.Features.FindAsync(id);
            if (feature == null)
            {
                return NotFound();
            }

            _context.Features.Remove(feature);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
