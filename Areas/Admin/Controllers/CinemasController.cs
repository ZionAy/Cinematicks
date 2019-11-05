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
	public class CinemasController : Controller
    {
        private readonly DBContext db;

        public CinemasController(DBContext context)
        {
            db = context;
        }

		/* List of cinemas */
		public async Task<IActionResult> Index()
		{
			var dbCinemas = db.Cinemas
				.Include(ci => ci.Photo)
				.Include(ci => ci.Halls)
				.Include(ci => ci.Gallery)
				.Include(ci => ci.Location);
			return View(await dbCinemas.ToListAsync());
		}

		/* List of cinemas after being filtered */
		[HttpPost]
		public async Task<IActionResult> Index(string find)
		{
			if (find == null) { return RedirectToAction(nameof(Index)); }
			else
			{
				find = find.Trim();
				var dbCinemas = db.Cinemas
					.Include(ci => ci.Photo)
					.Include(ci => ci.Halls)
					.Include(ci => ci.Gallery)
					.Include(ci => ci.Location)
					.Where(ci => ci.Name.Contains(find));
				return View(await dbCinemas.ToListAsync());
			}
		}

		/* Info of a cinema */
		public async Task<IActionResult> Info(int? id)
		{
			if (id == null) { return RedirectToAction("Page404", "Home"); }
			var dbCinema = await db.Cinemas
				.Include(ci => ci.Photo)
				.Include(ci => ci.Halls)
				.Include(ci => ci.Gallery).ThenInclude(ph => ph.Photo)
				.Include(ci => ci.Location).ThenInclude(l => l.Map)
				.SingleOrDefaultAsync(ci => ci.ID == id);
			if (dbCinema == null) { return RedirectToAction("Page404", "Home"); }
			return View(dbCinema);
		}

		/* Create a new cinema */
		public IActionResult Create()
		{
			ViewData["ImageID"] = new SelectList(db.Images.Where(i => i.Category == ImageCategory.Gallery), "ID", "Name");
			ViewData["LocationID"] = new SelectList(db.Locations, "ID", "FullAddress");
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create([Bind("Name,About,Price,LocationID,PhotoID")] Cinema cinema)
		{
			CinemaDBExist(cinema.Name);
			if (ModelState.IsValid)
			{
				try
				{
					db.Cinemas.Add(cinema);
					await db.SaveChangesAsync();
					TempData["Status"] = "A new cinema named "+ cinema.Name +" has been created.";
					TempData["Color"] = "success";
					return RedirectToAction(nameof(Index));
				}
				catch (Exception)
				{
					TempData["Status"] = "We're sorry, an unexpected error has been accured."+Environment.NewLine+"If you keep getting this error please contact system administrator.";
					TempData["Color"] = "danger";
					ViewData["ImageID"] = new SelectList(db.Images.Where(i => i.Category == ImageCategory.Gallery), "ID", "Name", cinema.PhotoID);
					ViewData["LocationID"] = new SelectList(db.Locations, "ID", "FullAddress", cinema.LocationID);
					return View(cinema);
				}
			}
			ViewData["ImageID"] = new SelectList(db.Images.Where(i => i.Category == ImageCategory.Gallery), "ID", "Name", cinema.PhotoID);
			ViewData["LocationID"] = new SelectList(db.Locations, "ID", "FullAddress", cinema.LocationID);
			return View(cinema);
		}

		/* Edit cinema */
		public async Task<IActionResult> Edit(int? id)
		{
			if (id == null) { return RedirectToAction("Page404", "Home"); }
			var dbCinema = await db.Cinemas
				.SingleOrDefaultAsync(ci => ci.ID == id);
			if (dbCinema == null) { return RedirectToAction("Page404", "Home"); }
			ViewData["ImageID"] = new SelectList(db.Images.Where(i => i.Category == ImageCategory.Gallery), "ID", "Name", dbCinema.PhotoID);
			ViewData["LocationID"] = new SelectList(db.Locations, "ID", "FullAddress", dbCinema.LocationID);
			return View(dbCinema);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(int id, [Bind("ID,Name,About,Price,LocationID,PhotoID")] Cinema cinema)
		{
			if (id != cinema.ID)
			{
				TempData["Status"] = "We're sorry, an unexpected error has been accured." + Environment.NewLine + "If you keep getting this error please contact system administrator.";
				TempData["Color"] = "danger";
				return RedirectToAction(nameof(Index));
			}
			CinemaDBExist(cinema.Name, id);
			if (ModelState.IsValid)
			{
				try
				{
					db.Cinemas.Update(cinema);
					await db.SaveChangesAsync();
					TempData["Status"] = "The changes you made to the cinema have been saved!";
					TempData["Color"] = "success";
				}
				catch (DbUpdateConcurrencyException)
				{
					if (!CinemaExists(cinema.ID)) { return RedirectToAction("Page404", "Home"); }
					TempData["Status"] = "We're sorry, an unexpected error has been accured." + Environment.NewLine + "If you keep getting this error please contact system administrator.";
					TempData["Color"] = "danger";
					return RedirectToAction(nameof(Index));
				}
				return RedirectToAction(nameof(Info), new { id = cinema.ID });
			}
			ViewData["ImageID"] = new SelectList(db.Images.Where(i => i.Category == ImageCategory.Gallery), "ID", "Name", cinema.PhotoID);
			ViewData["LocationID"] = new SelectList(db.Locations, "ID", "FullAddress", cinema.LocationID);
			return View(cinema);
		}


		/* Delete cinema */
		public async Task<IActionResult> Delete(int? id)
		{
			if (id == null) { return RedirectToAction("Page404", "Home"); }
			var dbCinema = await db.Cinemas
				.Include(ci => ci.Photo)
				.Include(ci => ci.Halls)
				.Include(ci => ci.Gallery)
				.Include(ci => ci.Location)
				.SingleOrDefaultAsync(ci => ci.ID == id);
			if (dbCinema == null) { return RedirectToAction("Page404", "Home"); }
			return View(dbCinema);
		}

		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(int id)
		{
			var dbCinema = await db.Cinemas
				.SingleOrDefaultAsync(ci => ci.ID == id);
			if (dbCinema == null) { return RedirectToAction("Page404", "Home"); }
			try
			{
				db.Cinemas.Remove(dbCinema);
				await db.SaveChangesAsync();
				TempData["Status"] = "The cinema has been successfully deleted!";
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
		private bool CinemaExists(int id)
		{
			return db.Cinemas.Any(e => e.ID == id);
		}

		/* Server Validations */
		private void CinemaDBExist(string cName, int? id = null)
		{
			// Unique name
			var dbCinema = db.Cinemas.AsNoTracking().FirstOrDefault(ci => ci.Name == cName);
			if ((dbCinema != null && id == null) || (dbCinema != null && dbCinema.ID != id))
			{
				ModelState.AddModelError("Name", "A cinema with that name already exists.");
			}
		}

		private void CinemaValidate(Cinema cinema, int? id = null)
		{
			// Unique name
			//var dbCinema = db.Cinemas.AsNoTracking().FirstOrDefault(ci => ci.Name == cName);
			//if ((dbCinema != null && id == null) || (dbCinema != null && dbCinema.ID != id))
			//{
			//	ModelState.AddModelError("Name", "A cinema with that name already exists.");
			//}
		}
	}
}
