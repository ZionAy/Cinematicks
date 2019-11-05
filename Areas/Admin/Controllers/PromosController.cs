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
	public class PromosController : Controller
    {
        private readonly DBContext db;
        public PromosController(DBContext context)
        {
            db = context;
        }


		/* List of deals */
		public async Task<IActionResult> Index()
		{
			var dbPromos = db.Promos
				.Include(d => d.Banner);
			return View(await dbPromos.ToListAsync());
		}

		/* List of cinemas after being filtered */
		[HttpPost]
		public async Task<IActionResult> Index(string find)
		{
			if (find == null) { return RedirectToAction(nameof(Index)); }
			else
			{
				find = find.Trim();
				var dbPromos = db.Promos
					.Include(d => d.Banner)
					.Where(d => d.Name.Contains(find));
				return View(await dbPromos.ToListAsync());
			}
		}

		/* Info of a deal */
		public async Task<IActionResult> Info(int? id)
		{
			if (id == null) { return RedirectToAction("Page404", "Home"); }
			var dbPromo = await db.Promos
				.Include(d => d.Banner)
				.SingleOrDefaultAsync(d => d.ID == id);
			if (dbPromo == null) { return RedirectToAction("Page404", "Home"); }
			return View(dbPromo);
		}

		/* Create new deal */
		public IActionResult Create()
		{
			ViewData["BannerID"] = new SelectList(db.Images.Where(i => i.Category == ImageCategory.Banners), "ID", "Name");
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create([Bind("Name,Description,StartTime,EndTime,BannerID")] Promo promo)
		{
			PromoDBExist(promo.Name);
			if (ModelState.IsValid)
			{
				try
				{
					db.Promos.Add(promo);
					await db.SaveChangesAsync();
					TempData["Status"] = "A new promo named " + promo.Name + " has been created.";
					TempData["Color"] = "success";
					return RedirectToAction(nameof(Index));
				}
				catch (Exception)
				{
					TempData["Status"] = "We're sorry, an unexpected error has been accured." + Environment.NewLine + "If you keep getting this error please contact system administrator.";
					TempData["Color"] = "danger";
					ViewData["BannerID"] = new SelectList(db.Images.Where(i => i.Category == ImageCategory.Banners), "ID", "Name", promo.BannerID);
					return View(promo);
				}
			}
			ViewData["BannerID"] = new SelectList(db.Images.Where(i => i.Category == ImageCategory.Banners), "ID", "Name", promo.BannerID);
			return View(promo);
		}


		/* Edit deal */
		public async Task<IActionResult> Edit(int? id)
		{
			if (id == null) { return RedirectToAction("Page404", "Home"); }
			var dbPromo = await db.Promos
				.SingleOrDefaultAsync(d => d.ID == id);
			if (dbPromo == null) { return RedirectToAction("Page404", "Home"); }
			ViewData["BannerID"] = new SelectList(db.Images.Where(i => i.Category == ImageCategory.Banners), "ID", "Name", dbPromo.BannerID);
			return View(dbPromo);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(int id, [Bind("ID,Name,Description,StartTime,EndTime,BannerID")] Promo promo)
		{
			if (id != promo.ID)
			{
				TempData["Status"] = "We're sorry, an unexpected error has been accured." + Environment.NewLine + "If you keep getting this error please contact system administrator.";
				TempData["Color"] = "danger";
				return RedirectToAction(nameof(Index));
			}
			PromoDBExist(promo.Name, promo.ID);
			if (ModelState.IsValid)
			{
				try
				{
					db.Promos.Update(promo);
					await db.SaveChangesAsync();
					TempData["Status"] = "The changes you made to the cinema have been saved!";
					TempData["Color"] = "success";
				}
				catch (DbUpdateConcurrencyException)
				{
					if (!PromoExists(promo.ID)) { return RedirectToAction("Page404", "Home"); }
					TempData["Status"] = "We're sorry, an unexpected error has been accured." + Environment.NewLine + "If you keep getting this error please contact system administrator.";
					TempData["Color"] = "danger";
					return RedirectToAction(nameof(Index));
				}
				return RedirectToAction(nameof(Info), new { id = promo.ID });
			}
			ViewData["BannerID"] = new SelectList(db.Images.Where(i => i.Category == ImageCategory.Banners), "ID", "Name", promo.BannerID);
			return View(promo);
		}


		/* Delete deal */
		public async Task<IActionResult> Delete(int? id)
		{
			if (id == null) { return RedirectToAction("Page404", "Home"); }
			var dbPromo = await db.Promos
				.Include(d => d.Banner)
				.SingleOrDefaultAsync(d => d.ID == id);
			if (dbPromo == null) { return RedirectToAction("Page404", "Home"); }
			return View(dbPromo);
		}

		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(int id)
		{
			var dbPromo = await db.Promos
				.SingleOrDefaultAsync(d => d.ID == id);
			if (dbPromo == null) { return RedirectToAction("Page404", "Home"); }
			try
			{
				db.Promos.Remove(dbPromo);
				await db.SaveChangesAsync();
				TempData["Status"] = "The promo has been successfully deleted!";
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
        private bool PromoExists(int id)
        {
            return db.Promos.Any(e => e.ID == id);
        }

		/* Server Validations */
		private void PromoDBExist(string cName, int? id = null)
		{
			// Unique name
			var dbPromo = db.Promos.AsNoTracking().FirstOrDefault(ci => ci.Name == cName);
			if ((dbPromo != null && id == null) || (dbPromo != null && dbPromo.ID != id))
			{
				ModelState.AddModelError("Name", "A promo with that name already exists.");
			}
		}
	}
}
