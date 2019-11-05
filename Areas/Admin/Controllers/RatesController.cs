using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Cinematicks.Models;
using Microsoft.AspNetCore.Authorization;

namespace Cinematicks.Areas.Admin.Controllers
{
    [Area("Admin")]
	[Authorize(Roles = "Admin")]
	public class RatesController : Controller
    {
        private readonly DBContext db;
        public RatesController(DBContext context)
        {
            db = context;
        }

		/* List of rates */
		public async Task<IActionResult> Index()
		{
			var dbRates = db.Rates
				.Include(rt => rt.Image)
				.Include(rt => rt.Movies);
			return View(await dbRates.ToListAsync());
		}

		/* List of rates after being filtered */
		[HttpPost]
		public async Task<IActionResult> Index(string find)
		{
			if (find == null) { return RedirectToAction(nameof(Index)); }
			else
			{
				find = find.Trim();
				var dbRates = db.Rates
					.Include(rt => rt.Image)
					.Include(rt => rt.Movies)
					.Where(r => r.Code.Contains(find) || r.Description.Contains(find));
				return View(await dbRates.ToListAsync());
			}
		}

		/* Info of a rate */
		public async Task<IActionResult> Info(int? id)
		{
			if (id == null) { return RedirectToAction("Page404", "Home"); }
			var dbRate = await db.Rates
				.Include(rt => rt.Image)
				.Include(rt => rt.Movies).ThenInclude(m => m.Poster)
				.SingleOrDefaultAsync(r => r.ID == id);
			if (dbRate == null) { return RedirectToAction("Page404", "Home"); }
			return View(dbRate);
		}

		/* Create new rate */
		public IActionResult Create()
		{
			ViewData["ImageID"] = new SelectList(db.Images.Where(i => i.Category == ImageCategory.Rates), "ID", "Name");
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create([Bind("Code,Description,MinAge,ImageID")] Rate rate)
		{
			RateDBExist(rate.Code);
			if (ModelState.IsValid)
			{
				try
				{
					db.Rates.Add(rate);
					await db.SaveChangesAsync();
					TempData["Status"] = "A new rate with code " + rate.Code + " has been created.";
					TempData["Color"] = "success";
					return RedirectToAction(nameof(Index));
				}
				catch (Exception)
				{
					TempData["Status"] = "We're sorry, an unexpected error has been accured." + Environment.NewLine + "If you keep getting this error please contact system administrator.";
					TempData["Color"] = "danger";
					ViewData["ImageID"] = new SelectList(db.Images.Where(i => i.Category == ImageCategory.Rates), "ID", "Name", rate.ImageID);
					return View(rate);
				}
			}
			ViewData["ImageID"] = new SelectList(db.Images.Where(i => i.Category == ImageCategory.Rates), "ID", "Name", rate.ImageID);
			return View(rate);
		}

		
		/* Edit rate */
		public async Task<IActionResult> Edit(int? id)
		{
			if (id == null) { return RedirectToAction("Page404", "Home"); }
			var dbRate = await db.Rates
				.SingleOrDefaultAsync(r => r.ID == id);
			if (dbRate == null) { return RedirectToAction("Page404", "Home"); }
			ViewData["ImageID"] = new SelectList(db.Images.Where(i => i.Category == ImageCategory.Rates), "ID", "Name", dbRate.ImageID);
			return View(dbRate);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(int id, [Bind("ID,Code,Description,MinAge,ImageID")] Rate rate)
		{
			if (id != rate.ID)
			{
				TempData["Status"] = "We're sorry, an unexpected error has been accured." + Environment.NewLine + "If you keep getting this error please contact system administrator.";
				TempData["Color"] = "danger";
				return RedirectToAction(nameof(Index));
			}
			RateDBExist(rate.Code, rate.ID);
			if (ModelState.IsValid)
			{
				try
				{
					db.Rates.Update(rate);
					await db.SaveChangesAsync();
					TempData["Status"] = "The changes you made to the cinema have been saved!";
					TempData["Color"] = "success";
				}
				catch (DbUpdateConcurrencyException)
				{
					if (!RateExists(rate.ID)) { return RedirectToAction("Page404", "Home"); }
					TempData["Status"] = "We're sorry, an unexpected error has been accured." + Environment.NewLine + "If you keep getting this error please contact system administrator.";
					TempData["Color"] = "danger";
					return RedirectToAction(nameof(Index));
				}
				return RedirectToAction(nameof(Info), new { id = rate.ID });
			}
			ViewData["ImageID"] = new SelectList(db.Images.Where(i => i.Category == ImageCategory.Rates), "ID", "Name", rate.ImageID);
			return View(rate);
		}

		/* Delete rate */
		public async Task<IActionResult> Delete(int? id)
		{
			if (id == null) { return RedirectToAction("Page404", "Home"); }
			var dbRate = await db.Rates
				.Include(r => r.Image)
				.Include(rt => rt.Movies)
				.SingleOrDefaultAsync(r => r.ID == id);
			if (dbRate == null) { return RedirectToAction("Page404", "Home"); }
			return View(dbRate);
		}

		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(int id)
		{
			var dbRate = await db.Rates
				.SingleOrDefaultAsync(r => r.ID == id);
			if (dbRate == null) { return RedirectToAction("Page404", "Home"); }
			try
			{
				db.Rates.Remove(dbRate);
				await db.SaveChangesAsync();
				TempData["Status"] = "The rate has been successfully deleted!";
				TempData["Color"] = "success";
			}
			catch (Exception)
			{
				TempData["Status"] = "We're sorry, an unexpected error has been accured." + Environment.NewLine + "If you keep getting this error please contact system administrator.";
				TempData["Color"] = "danger";
			}			
			return RedirectToAction(nameof(Index));
		}

		// Check if exists in DB
        private bool RateExists(int id)
        {
            return db.Rates.Any(e => e.ID == id);
        }

		/* Server Validations */
		private void RateDBExist(string rCode, int? id = null)
		{
			// Unique code
			var dbRate = db.Rates.AsNoTracking().FirstOrDefault(rt => rt.Code == rCode);
			if ((dbRate != null && id == null) || (dbRate != null && dbRate.ID != id))
			{
				ModelState.AddModelError("Code", "A rate with that code already exists.");
			}
		}
	}
}
