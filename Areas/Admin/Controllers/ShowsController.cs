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
	public class ShowsController : Controller
    {
        private readonly DBContext db;
        public ShowsController(DBContext context)
        {
            db = context;
        }


		/* List of shows */
		public async Task<IActionResult> Index()
		{
			var dbShows = db.Shows
				.Include(s => s.Hall).ThenInclude(h => h.Cinema)
				.Include(s => s.Movie).ThenInclude(m => m.Poster)
				.Include(s => s.Tickets)
				.OrderByDescending(o => o.ShowDate).ThenByDescending(o => o.ShowTime);
			return View(await dbShows.ToListAsync());
		}

		/* List of shows after being filtered */
		[HttpPost]
		public async Task<IActionResult> Index(string find)
		{
			if (find == null) { return RedirectToAction(nameof(Index)); }
			else
			{
				find = find.Trim();
				var dbShows = db.Shows
					.Include(s => s.Hall).ThenInclude(h => h.Cinema)
					.Include(s => s.Movie).ThenInclude(m => m.Poster)
					.Include(s => s.Tickets)
					.Where(s => s.Movie.Title.Contains(find) || s.Hall.Name.Contains(find));
				return View(await dbShows.ToListAsync());
			}
		}

		/* Info of a show */
		public async Task<IActionResult> Info(int? id)
		{
			if (id == null) { return RedirectToAction("Page404", "Home"); }
			var dbShow = await db.Shows
				.Include(s => s.Hall).ThenInclude(h => h.Cinema)
				.Include(s => s.Movie).ThenInclude(m => m.Poster)
				.Include(s => s.Tickets)
				.SingleOrDefaultAsync(s => s.ID == id);
			if (dbShow == null) { return RedirectToAction("Page404", "Home"); }
			return View(dbShow);
		}

		/* Create new show */
		public IActionResult Create()
		{
			ViewData["HallID"] = new SelectList(db.Halls.Select(h => new { h.ID, Name = $"{h.Name} - {h.Cinema.Name}" }).ToList(), "ID", "Name");
			ViewData["MovieID"] = new SelectList(db.Movies, "ID", "Title");
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create([Bind("HallID,MovieID,ShowDate,ShowTime")] Show show)
		{
			ShowDBExist(show.ShowDate, show.ShowTime, show.HallID);
			if (ModelState.IsValid)
			{
				try
				{
					db.Shows.Add(show);
					await db.SaveChangesAsync();
					TempData["Status"] = "A new show has been created.";
					TempData["Color"] = "success";
					return RedirectToAction(nameof(Index));
				}
				catch (Exception)
				{
					TempData["Status"] = "We're sorry, an unexpected error has been accured." + Environment.NewLine + "If you keep getting this error please contact system administrator.";
					TempData["Color"] = "danger";
					ViewData["HallID"] = new SelectList(db.Halls.Select(h => new { h.ID, Name = $"{h.Name} - {h.Cinema.Name}" }).ToList(), "ID", "Name", show.HallID);
					ViewData["MovieID"] = new SelectList(db.Movies, "ID", "Title", show.MovieID);
					return View(show);
				}
			}
			ViewData["HallID"] = new SelectList(db.Halls.Select(h => new { h.ID, Name = $"{h.Name} - {h.Cinema.Name}" }).ToList(), "ID", "Name", show.HallID);
			ViewData["MovieID"] = new SelectList(db.Movies, "ID", "Title", show.MovieID);
			return View(show);
		}


		/* Edit show */
		public async Task<IActionResult> Edit(int? id)
		{
			if (id == null) { return RedirectToAction("Page404", "Home"); }
			var dbShow = await db.Shows
				.SingleOrDefaultAsync(s => s.ID == id);
			if (dbShow == null) { return RedirectToAction("Page404", "Home"); }
			ViewData["HallID"] = new SelectList(db.Halls.Select(h => new { h.ID, Name = $"{h.Name} - {h.Cinema.Name}" }).ToList(), "ID", "Name", dbShow.HallID);
			ViewData["MovieID"] = new SelectList(db.Movies, "ID", "Title", dbShow.MovieID);
			return View(dbShow);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(int id, [Bind("ID,HallID,MovieID,ShowDate,ShowTime")] Show show)
		{
			if (id != show.ID)
			{
				TempData["Status"] = "We're sorry, an unexpected error has been accured." + Environment.NewLine + "If you keep getting this error please contact system administrator.";
				TempData["Color"] = "danger";
				return RedirectToAction(nameof(Index));
			}
			ShowDBExist(show.ShowDate, show.ShowTime, show.HallID, show.ID);
			if (ModelState.IsValid)
			{
				try
				{
					db.Shows.Update(show);
					await db.SaveChangesAsync();
					TempData["Status"] = "The changes you made to the show have been saved!";
					TempData["Color"] = "success";
				}
				catch (DbUpdateConcurrencyException)
				{
					if (!ShowExists(show.ID)) { return RedirectToAction("Page404", "Home"); }
					TempData["Status"] = "We're sorry, an unexpected error has been accured." + Environment.NewLine + "If you keep getting this error please contact system administrator.";
					TempData["Color"] = "danger";
					return RedirectToAction(nameof(Index));
				}
				return RedirectToAction(nameof(Info), new { id = show.ID });
			}
			ViewData["HallID"] = new SelectList(db.Halls.Select(h => new { h.ID, Name = $"{h.Name} - {h.Cinema.Name}" }).ToList(), "ID", "Name", show.HallID);
			ViewData["MovieID"] = new SelectList(db.Movies, "ID", "Title", show.MovieID);
			return View(show);
		}



		/* Delete show */
		public async Task<IActionResult> Delete(int? id)
		{
			if (id == null) { return RedirectToAction("Page404", "Home"); }
			var dbShow = await db.Shows
				.Include(s => s.Hall).ThenInclude(h => h.Cinema)
				.Include(s => s.Movie).ThenInclude(m => m.Poster)
				.Include(s => s.Tickets)
				.SingleOrDefaultAsync(s => s.ID == id);
			if (dbShow == null) { return RedirectToAction("Page404", "Home"); }
			return View(dbShow);
		}

		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(int id)
		{
			var dbShow = await db.Shows
				.SingleOrDefaultAsync(s => s.ID == id);
			if (dbShow == null) { return RedirectToAction("Page404", "Home"); }
			try
			{
				db.Shows.Remove(dbShow);
				await db.SaveChangesAsync();
				TempData["Status"] = "The show has been successfully deleted!";
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
		private bool ShowExists(int id)
        {
            return db.Shows.Any(e => e.ID == id);
        }

		/* Server Validations */
		private void ShowDBExist(DateTime sDate, TimeSpan sTime, int sHall, int? id = null)
		{
			// Hall is in use on this time
			var dbShow = db.Shows.AsNoTracking().LastOrDefault(s => s.HallID == sHall && s.ShowDate == sDate && s.ShowTime <= sTime);
			if (dbShow.ShowTime.TotalMinutes + 180 > sTime.TotalMinutes)
			{
				if ((dbShow != null && id == null) || (dbShow != null && dbShow.ID != id))
				{
					ModelState.AddModelError("ShowTime", "There is a show on that hall on this time.");
				}
			}			
		}
	}
}
