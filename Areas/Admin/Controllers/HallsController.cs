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
	public class HallsController : Controller
    {
        private readonly DBContext db;
        public HallsController(DBContext context)
        {
            db = context;
        }


		/* List of halls */
		public async Task<IActionResult> Index()
		{
			var dbHalls = db.Halls
				.Include(h => h.Cinema)
				.Include(h => h.Shows);
			return View(await dbHalls.ToListAsync());
		}

		/* List of halls after being filtered */
		[HttpPost]
		public async Task<IActionResult> Index(string find)
		{
			if (find == null) { return RedirectToAction(nameof(Index)); }
			else
			{
				find = find.Trim();
				var dbHalls = db.Halls
					.Include(h => h.Cinema)
					.Include(h => h.Shows)
					.Where(h => h.Name.Contains(find) || h.Cinema.Name.Contains(find));
				return View(await dbHalls.ToListAsync());
			}
		}
		

		/* Info of a hall */
		public async Task<IActionResult> Info(int? id)
		{
			if (id == null) { return RedirectToAction("Page404", "Home"); }
			var dbHall = await db.Halls
				.Include(h => h.Cinema)
				.Include(h => h.Shows).ThenInclude(s => s.Tickets)
				.Include(h => h.Shows).ThenInclude(s => s.Movie)
				.SingleOrDefaultAsync(h => h.ID == id);
			if (dbHall == null) { return RedirectToAction("Page404", "Home"); }
			return View(dbHall);
		}

		/* Create new hall */
		public IActionResult Create()
		{
			ViewData["CinemaID"] = new SelectList(db.Cinemas, "ID", "Name");
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create([Bind("Name,Rows,Cols,CinemaID")] Hall hall)
		{
			HallDBExist(hall.Name, hall.CinemaID);
			if (ModelState.IsValid)
			{
				try
				{
					db.Halls.Add(hall);
					await db.SaveChangesAsync();
					TempData["Status"] = "A new hall named " + hall.Name + " has been created.";
					TempData["Color"] = "success";
					return RedirectToAction(nameof(Index));
				}
				catch (Exception)
				{
					TempData["Status"] = "We're sorry, an unexpected error has been accured." + Environment.NewLine + "If you keep getting this error please contact system administrator.";
					TempData["Color"] = "danger";
					ViewData["CinemaID"] = new SelectList(db.Cinemas, "ID", "Name", hall.CinemaID);
					return View(hall);
				}
			}
			ViewData["CinemaID"] = new SelectList(db.Cinemas, "ID", "Name", hall.CinemaID);
			return View(hall);
		}


		/* Edit hall */
		public async Task<IActionResult> Edit(int? id)
		{
			if (id == null) { return RedirectToAction("Page404", "Home"); }
			var dbHall = await db.Halls
				.SingleOrDefaultAsync(h => h.ID == id);
			if (dbHall == null) { return RedirectToAction("Page404", "Home"); }
			ViewData["CinemaID"] = new SelectList(db.Cinemas, "ID", "Name", dbHall.CinemaID);
			return View(dbHall);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(int id, [Bind("ID,Name,Rows,Cols,CinemaID")] Hall hall)
		{
			if (id != hall.ID)
			{
				TempData["Status"] = "We're sorry, an unexpected error has been accured." + Environment.NewLine + "If you keep getting this error please contact system administrator.";
				TempData["Color"] = "danger";
				return RedirectToAction(nameof(Index));
			}
			HallDBExist(hall.Name, hall.CinemaID, hall.ID);
			if (ModelState.IsValid)
			{
				try
				{
					db.Halls.Update(hall);
					await db.SaveChangesAsync();
					TempData["Status"] = "The changes you made to the hall have been saved!";
					TempData["Color"] = "success";
				}
				catch (DbUpdateConcurrencyException)
				{
					if (!HallExists(hall.ID)) { return RedirectToAction("Page404", "Home"); }
					TempData["Status"] = "We're sorry, an unexpected error has been accured." + Environment.NewLine + "If you keep getting this error please contact system administrator.";
					TempData["Color"] = "danger";
					return RedirectToAction(nameof(Index));
				}
				return RedirectToAction(nameof(Info), new { id = hall.ID });
			}
			ViewData["CinemaID"] = new SelectList(db.Cinemas, "ID", "Name", hall.CinemaID);
			return View(hall);
		}


		/* Delete hall */
		public async Task<IActionResult> Delete(int? id)
		{
			if (id == null) { return RedirectToAction("Page404", "Home"); }
			var dbHall = await db.Halls
				.Include(h => h.Cinema)
				.Include(h => h.Shows)
				.SingleOrDefaultAsync(h => h.ID == id);
			if (dbHall == null) { return RedirectToAction("Page404", "Home"); }
			return View(dbHall);
		}

		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(int id)
		{
			var dbHall = await db.Halls
				.SingleOrDefaultAsync(h => h.ID == id);
			if (dbHall == null) { return RedirectToAction("Page404", "Home"); }
			try
			{
				db.Halls.Remove(dbHall);
				await db.SaveChangesAsync();
				TempData["Status"] = "The hall has been successfully deleted!";
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
		private bool HallExists(int id)
		{
			return db.Halls.Any(h => h.ID == id);
		}

		/* Server Validations */
		private void HallDBExist(string hName, int hCinema, int? id = null)
		{
			// Unique name
			var dbHall = db.Halls.AsNoTracking().FirstOrDefault(h => h.Name == hName && h.CinemaID == hCinema);
			if ((dbHall != null && id == null) || (dbHall != null && dbHall.ID != id))
			{
				ModelState.AddModelError("Name", "A Hall with that name already exists on this cinema.");
			}
		}
	}
}
