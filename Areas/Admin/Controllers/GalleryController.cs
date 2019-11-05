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
	public class GalleryController : Controller
    {
        private readonly DBContext db;
        public GalleryController(DBContext context)
        {
            db = context;
        }

		/* List of gallery photos */
		public async Task<IActionResult> Index()
		{
			var dbPhotos = db.Gallery
				.Include(ph => ph.Cinema)
				.Include(ph => ph.Photo);
			return View(await dbPhotos.ToListAsync());
		}

		/* List of gallery photos after being filtered */
		[HttpPost]
		public async Task<IActionResult> Index(string find)
		{
			if (find == null) { return RedirectToAction(nameof(Index)); }
			else
			{
				find = find.Trim();
				var dbPhotos = db.Gallery
					.Include(ph => ph.Cinema)
					.Include(ph => ph.Photo)
					.Where(ph => ph.Title.Contains(find) || ph.Description.Contains(find));
				return View(await dbPhotos.ToListAsync());
			}
		}

		/* Info of a cinema */
		public async Task<IActionResult> Info(int? id)
		{
			if (id == null) { return RedirectToAction("Page404", "Home"); }
			var dbPhoto = await db.Gallery
				.Include(p => p.Cinema)
				.Include(p => p.Photo)
				.SingleOrDefaultAsync(p => p.ID == id);
			if (dbPhoto == null) { return RedirectToAction("Page404", "Home"); }
			return View(dbPhoto);
		}

		/* Create new cinema */
		public IActionResult Create()
		{
			ViewData["CinemaID"] = new SelectList(db.Cinemas, "ID", "Name");
			ViewData["PhotoID"] = new SelectList(db.Images.Where(i => i.Category == ImageCategory.Gallery), "ID", "Name");
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create([Bind("Title,Description,IsActive,PhotoID,CinemaID")] PhotoGal photo)
		{
			PhotoDBExist(photo.Title, photo.CinemaID);
			if (ModelState.IsValid)
			{
				try
				{
					db.Gallery.Add(photo);
					await db.SaveChangesAsync();
					TempData["Status"] = "A new photo named " + photo.Title + " has been added to the gallery.";
					TempData["Color"] = "success";
					return RedirectToAction(nameof(Index));
				}
				catch (Exception)
				{
					TempData["Status"] = "We're sorry, an unexpected error has been accured." + Environment.NewLine + "If you keep getting this error please contact system administrator.";
					TempData["Color"] = "danger";
					ViewData["CinemaID"] = new SelectList(db.Cinemas, "ID", "Name", photo.CinemaID);
					ViewData["PhotoID"] = new SelectList(db.Images.Where(i => i.Category == ImageCategory.Gallery), "ID", "Name", photo.PhotoID);
					return View(photo);
				}

			}
			ViewData["CinemaID"] = new SelectList(db.Cinemas, "ID", "Name", photo.CinemaID);
			ViewData["PhotoID"] = new SelectList(db.Images.Where(i => i.Category == ImageCategory.Gallery), "ID", "Name", photo.PhotoID);
			return View(photo);
		}

		/* Edit cinema */
		public async Task<IActionResult> Edit(int? id)
		{
			if (id == null) { return RedirectToAction("Page404", "Home"); }
			var dbPhoto = await db.Gallery
				.SingleOrDefaultAsync(p => p.ID == id);
			if (dbPhoto == null) { return RedirectToAction("Page404", "Home"); }
			ViewData["CinemaID"] = new SelectList(db.Cinemas, "ID", "Name", dbPhoto.CinemaID);
			ViewData["PhotoID"] = new SelectList(db.Images.Where(i => i.Category == ImageCategory.Gallery), "ID", "Name", dbPhoto.PhotoID);
			return View(dbPhoto);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(int id, [Bind("ID,Title,Description,IsActive,PhotoID,CinemaID")] PhotoGal photo)
		{
			if (id != photo.ID)
			{
				TempData["Status"] = "We're sorry, an unexpected error has been accured." + Environment.NewLine + "If you keep getting this error please contact system administrator.";
				TempData["Color"] = "danger";
				return RedirectToAction(nameof(Index));
			}
			PhotoDBExist(photo.Title, photo.CinemaID, photo.ID);
			if (ModelState.IsValid)
			{
				try
				{
					db.Gallery.Update(photo);
					await db.SaveChangesAsync();
					TempData["Status"] = "The changes you made to the photo have been saved!";
					TempData["Color"] = "success";
				}
				catch (DbUpdateConcurrencyException)
				{
					if (!PhotoGalExists(photo.ID)) { return RedirectToAction("Page404", "Home"); }
					TempData["Status"] = "We're sorry, an unexpected error has been accured." + Environment.NewLine + "If you keep getting this error please contact system administrator.";
					TempData["Color"] = "danger";
					return RedirectToAction(nameof(Index));
				}
				return RedirectToAction(nameof(Info), new { id = photo.ID });
			}
			ViewData["CinemaID"] = new SelectList(db.Cinemas, "ID", "Name", photo.CinemaID);
			ViewData["PhotoID"] = new SelectList(db.Images.Where(i => i.Category == ImageCategory.Gallery), "ID", "Name", photo.PhotoID);
			return View(photo);
		}


		/* Delete cinema */
		public async Task<IActionResult> Delete(int? id)
		{
			if (id == null) { return RedirectToAction("Page404", "Home"); }
			var dbPhoto = await db.Gallery
				.Include(p => p.Cinema)
				.Include(p => p.Photo)
				.SingleOrDefaultAsync(p => p.ID == id);
			if (dbPhoto == null) { return RedirectToAction("Page404", "Home"); }
			return View(dbPhoto);
		}

		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(int id)
		{
			var dbPhoto = await db.Gallery
				.SingleOrDefaultAsync(p => p.ID == id);
			if (dbPhoto == null) { return RedirectToAction("Page404", "Home"); }
			try
			{
				db.Gallery.Remove(dbPhoto);
				await db.SaveChangesAsync();
				TempData["Status"] = "The photo has been successfully removed from the gallery!";
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
        private bool PhotoGalExists(int id)
        {
            return db.Gallery.Any(e => e.ID == id);
        }


		/* Server Validations */
		private void PhotoDBExist(string pTitle, int pCinema, int? id = null)
		{
			// Unique name and cinema
			var dbPhoto = db.Gallery.AsNoTracking().FirstOrDefault(ph => ph.Title == pTitle && ph.CinemaID == pCinema);
			if ((dbPhoto != null && id == null) || (dbPhoto != null && dbPhoto.ID != id))
			{
				ModelState.AddModelError("Title", "A photo with that name already exists on this cinema gallery.");
			}
		}
	}
}
