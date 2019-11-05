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
	public class GenresController : Controller
    {
        private readonly DBContext db;
        public GenresController(DBContext context)
        {
            db = context;
        }

		/* List of genres */
		public async Task<IActionResult> Index()
		{
			var dbGenres = db.Genres
				.Include(g => g.Movies);
			return View(await dbGenres.ToListAsync());
		}

		/* List of genres after being filtered */
		[HttpPost]
		public async Task<IActionResult> Index(string find)
		{
			if (find == null) { return RedirectToAction(nameof(Index)); }
			else
			{
				find = find.Trim();
				var dbGenres = db.Genres
					.Include(g => g.Movies)
					.Where(g => g.Name.Contains(find));
				return View(await dbGenres.ToListAsync());
			}
		}

		/* Info of a genre */
		public async Task<IActionResult> Info(int? id)
		{
			if (id == null) { return RedirectToAction("Page404", "Home"); }
			var dbGenre = await db.Genres
				.Include(g => g.Movies).ThenInclude(mg => mg.Movie).ThenInclude(m => m.Poster)
				.Include(g => g.Movies).ThenInclude(mg => mg.Movie).ThenInclude(m => m.Rate)
				.SingleOrDefaultAsync(g => g.ID == id);
			if (dbGenre == null) { return RedirectToAction("Page404", "Home"); }
			return View(dbGenre);
		}

		/* Create new genre */
		public IActionResult Create()
		{
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create([Bind("Name,Description,InMenu")] Genre genre)
		{
			GenreDBExist(genre.Name);
			if (ModelState.IsValid)
			{
				try
				{
					db.Genres.Add(genre);
					await db.SaveChangesAsync();
					TempData["Status"] = "A new genre named " + genre.Name + " has been created.";
					TempData["Color"] = "success";
					return RedirectToAction(nameof(Index));
				}
				catch (Exception)
				{
					TempData["Status"] = "We're sorry, an unexpected error has been accured." + Environment.NewLine + "If you keep getting this error please contact system administrator.";
					TempData["Color"] = "danger";
					return View(genre);
				}
			}
			return View(genre);
		}


		/* Edit genre */
		public async Task<IActionResult> Edit(int? id)
		{
			if (id == null) { return RedirectToAction("Page404", "Home"); }
			var dbGenre = await db.Genres
				.SingleOrDefaultAsync(g => g.ID == id);
			if (dbGenre == null) { return RedirectToAction("Page404", "Home"); }
			return View(dbGenre);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(int id, [Bind("ID,Name,Description,InMenu")] Genre genre)
		{
			if (id != genre.ID)
			{
				TempData["Status"] = "We're sorry, an unexpected error has been accured." + Environment.NewLine + "If you keep getting this error please contact system administrator.";
				TempData["Color"] = "danger";
				return RedirectToAction(nameof(Index));
			}
			GenreDBExist(genre.Name, genre.ID);
			if (ModelState.IsValid)
			{
				try
				{
					db.Genres.Update(genre);
					await db.SaveChangesAsync();
					TempData["Status"] = "The changes you made to the genre have been saved!";
					TempData["Color"] = "success";
				}
				catch (DbUpdateConcurrencyException)
				{
					if (!GenreExists(genre.ID)) { return RedirectToAction("Page404", "Home"); }
					TempData["Status"] = "We're sorry, an unexpected error has been accured." + Environment.NewLine + "If you keep getting this error please contact system administrator.";
					TempData["Color"] = "danger";
					return RedirectToAction(nameof(Index));
				}
				return RedirectToAction(nameof(Info), new { id = genre.ID });
			}
			return View(genre);
		}


		/* Delete genre */
		public async Task<IActionResult> Delete(int? id)
		{
			if (id == null) { return RedirectToAction("Page404", "Home"); }
			var dbGenre = await db.Genres
				.Include(g => g.Movies)
				.SingleOrDefaultAsync(g => g.ID == id);
			if (dbGenre == null) { return RedirectToAction("Page404", "Home"); }
			return View(dbGenre);
		}

		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(int id)
		{
			var dbGenre = await db.Genres
				.SingleOrDefaultAsync(g => g.ID == id);
			try
			{
				db.Genres.Remove(dbGenre);
				await db.SaveChangesAsync();
				TempData["Status"] = "The genre has been successfully deleted!";
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
        private bool GenreExists(int id)
        {
            return db.Genres.Any(e => e.ID == id);
        }

		/* Server Validations */
		private void GenreDBExist(string gName, int? id = null)
		{
			// Unique name
			var dbGenre = db.Genres.AsNoTracking().FirstOrDefault(g => g.Name == gName);
			if ((dbGenre != null && id == null) || (dbGenre != null && dbGenre.ID != id))
			{
				ModelState.AddModelError("Name", "A genre with that name already exists.");
			}
		}
	}
}
