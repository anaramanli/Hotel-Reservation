using Hotel.DAL;
using Hotel.Extensions;
using Hotel.Models;
using Hotel.ViewModels.AboutCompany;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Hotel.Areas.Admin.Controllers
{
	[Area("Admin")]
	public class AboutCompanyController(HotelDBContext _context, IWebHostEnvironment _env) : Controller
	{
		// GET: AboutCompanyController
		public async Task<ActionResult> Index(int page = 0)
		{
			int pageSize = 4;
			var totalItems = await _context.AboutCompanies.CountAsync();
			ViewBag.MaxPage = (int)Math.Ceiling((double)totalItems / pageSize);
			ViewBag.CurrentPage = page;

			var data = await _context.AboutCompanies.Skip(page * pageSize).Take(pageSize)
				.Select(ac => new AboutCompanyGetAdminVM
				{
					Id = ac.Id,
					CeoName = ac.CeoName,
					CeoSignatureUrl = ac.CeoSignatureUrl,
					Description = ac.Description,
					Icon = ac.Icon,
					ImageUrlOne = ac.ImageUrlOne,
					ImageUrlTwo = ac.ImageUrlTwo,
					LatestVideos = ac.LatestVideos,
					OurServiceOne = ac.OurServiceOne,
					OurServiceTwo = ac.OurServiceTwo,
					ServiceDescriptionOne = ac.ServiceDescriptionOne,
					ServiceDescriptionTwo = ac.ServiceDescriptionTwo,
					Title = ac.Title,
					CeoImageUrl = ac.CeoImageUrl
				}).ToListAsync();
			return View(data);
		}

		// GET: AboutCompanyController/Create
		public ActionResult Create()
		{
			return View();
		}

		// POST: AboutCompanyController/Create
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(ACCreateAdminVM vm)
		{
			if (ModelState.IsValid)
			{
				if (vm.ImageUrlOne != null || vm.ImageUrlTwo != null || vm.CeoImage != null || vm.CeoSignature != null)
				{
					if (vm.ImageUrlOne != null && (!vm.ImageUrlOne.IsValidType("image") || !vm.ImageUrlOne.IsValidLength(2000)))
						ModelState.AddModelError("ImageUrlOne", "File must be an image and size must be lower than 2MB.");

					if (vm.ImageUrlTwo != null && (!vm.ImageUrlTwo.IsValidType("image") || !vm.ImageUrlTwo.IsValidLength(2000)))
						ModelState.AddModelError("ImageUrlTwo", "File must be an image and size must be lower than 2MB.");

					if (vm.CeoImage != null && (!vm.CeoImage.IsValidType("image") || !vm.CeoImage.IsValidLength(2000)))
						ModelState.AddModelError("CeoImage", "File must be an image and size must be lower than 2MB.");

					if (vm.CeoSignature != null && (!vm.CeoSignature.IsValidType("image") || !vm.CeoSignature.IsValidLength(2000)))
						ModelState.AddModelError("CeoSignature", "File must be an image and size must be lower than 2MB.");

					if (ModelState.IsValid)
					{
						string fileNameOne = vm.ImageUrlOne != null ? await vm.ImageUrlOne.SaveFileAsync(Path.Combine(_env.WebRootPath, "assets", "imgs")) : null;
						string fileNameTwo = vm.ImageUrlTwo != null ? await vm.ImageUrlTwo.SaveFileAsync(Path.Combine(_env.WebRootPath, "assets", "imgs")) : null;
						string fileNameCeo = vm.CeoImage != null ? await vm.CeoImage.SaveFileAsync(Path.Combine(_env.WebRootPath, "assets", "imgs")) : null;
						string fileNameCeoSign = vm.CeoSignature != null ? await vm.CeoSignature.SaveFileAsync(Path.Combine(_env.WebRootPath, "assets", "imgs")) : null;

						AboutCompany company = new AboutCompany
						{
							CreatedAt = DateTime.Now,
							ImageUrlOne = fileNameOne,
							ImageUrlTwo = fileNameTwo,
							CeoImageUrl = fileNameCeo,
							CeoSignatureUrl = fileNameCeoSign,
							Title = vm.Title,
							CeoName = vm.CeoName,
							Description = vm.Description,
							Icon = vm.Icon,
							LatestVideos = vm.LatestVideos,
							OurServiceOne = vm.OurServiceOne,
							OurServiceTwo = vm.OurServiceTwo,
							ServiceDescriptionOne = vm.ServiceDescriptionOne,
							ServiceDescriptionTwo = vm.ServiceDescriptionTwo,
							IsDeleted = false,
						};
						await _context.AddAsync(company);
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

		//GET: AboutCompanyController/Edit/5
		public async Task<IActionResult> Edit(int? id)
		{
			if (id == null || id < 1)
				return BadRequest();

			var data = await _context.AboutCompanies.FirstOrDefaultAsync(ac => ac.Id == id);
			if (data == null)
				return NotFound();

			var vm = new ACEditAdminVM
			{
				Title = data.Title,
				CeoImageUrl = data.CeoImageUrl,
				CeoSignatureUrl = data.CeoSignatureUrl,
				ImageUrlOne = data.ImageUrlOne,
				ImageUrlTwo = data.ImageUrlTwo,
				CeoName = data.CeoName,
				Description = data.Description,
				Icon = data.Icon,
				LatestVideos = data.LatestVideos,
				OurServiceOne = data.OurServiceOne,
				OurServiceTwo = data.OurServiceTwo,
				ServiceDescriptionOne = data.ServiceDescriptionOne,
				ServiceDescriptionTwo = data.ServiceDescriptionTwo,
			};
			return View(vm);
		}

		// POST: AboutCompanyController/Edit/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(int id, ACEditAdminVM vm)
		{
			if (!ModelState.IsValid)
			{
				return View(vm);
			}

			var company = await _context.AboutCompanies.FirstOrDefaultAsync(s => s.Id == id);
			if (company == null)
				return NotFound();

			company.Title = vm.Title;
			company.CeoName = vm.CeoName;
			company.Description = vm.Description;
			company.Icon = vm.Icon;
			company.LatestVideos = vm.LatestVideos;
			company.OurServiceOne = vm.OurServiceOne;
			company.OurServiceTwo = vm.OurServiceTwo;
			company.ServiceDescriptionOne = vm.ServiceDescriptionOne;
			company.ServiceDescriptionTwo = vm.ServiceDescriptionTwo;

			if (vm.ImageFileOne != null)
			{
				if (!string.IsNullOrEmpty(vm.ImageUrlOne))
				{
					string filePathOne = Path.Combine(_env.WebRootPath, "assets", "imgs", vm.ImageUrlOne);
					if (System.IO.File.Exists(filePathOne))
					{
						System.IO.File.Delete(filePathOne);
					}
				}
				string newFileNameOne = await vm.ImageFileOne.SaveFileAsync(Path.Combine(_env.WebRootPath, "assets", "imgs"));
				company.ImageUrlOne = newFileNameOne;
			}

			if (vm.ImageFileTwo != null)
			{
				if (!string.IsNullOrEmpty(vm.ImageUrlTwo))
				{
					string filePathTwo = Path.Combine(_env.WebRootPath, "assets", "imgs", vm.ImageUrlTwo);
					if (System.IO.File.Exists(filePathTwo))
					{
						System.IO.File.Delete(filePathTwo);
					}
				}
				string newFileNameTwo = await vm.ImageFileTwo.SaveFileAsync(Path.Combine(_env.WebRootPath, "assets", "imgs"));
				company.ImageUrlTwo = newFileNameTwo;
			}

			if (vm.CeoImage != null)
			{
				if (!string.IsNullOrEmpty(vm.CeoImageUrl))
				{
					string filePathCeo = Path.Combine(_env.WebRootPath, "assets", "imgs", vm.CeoImageUrl);
					if (System.IO.File.Exists(filePathCeo))
					{
						System.IO.File.Delete(filePathCeo);
					}
				}
				string newFileNameCeo = await vm.CeoImage.SaveFileAsync(Path.Combine(_env.WebRootPath, "assets", "imgs"));
				company.CeoImageUrl = newFileNameCeo;
			}

			if (vm.CeoSignature != null)
			{
				if (!string.IsNullOrEmpty(vm.CeoSignatureUrl))
				{
					string filePathCeoSign = Path.Combine(_env.WebRootPath, "assets", "imgs", vm.CeoSignatureUrl);
					if (System.IO.File.Exists(filePathCeoSign))
					{
						System.IO.File.Delete(filePathCeoSign);
					}
				}
				string newFileNameCeoSign = await vm.CeoSignature.SaveFileAsync(Path.Combine(_env.WebRootPath, "assets", "imgs"));
				company.CeoSignatureUrl = newFileNameCeoSign;
			}

			_context.AboutCompanies.Update(company);
			await _context.SaveChangesAsync();

			return RedirectToAction(nameof(Index));
		}

		// GET: AboutCompanyController/Delete/5
		public async Task<IActionResult> Delete(int id)
		{
			var company = await _context.AboutCompanies.FindAsync(id);
			if (company != null)
			{
				_context.AboutCompanies.Remove(company);
				await _context.SaveChangesAsync();
			}
			return RedirectToAction(nameof(Index));
		}
	}
}
