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
	public class LocationsController : Controller
    {
        private readonly DBContext db;
        public LocationsController(DBContext context)
        {
            db = context;
        }

		/* List of locations */
		public async Task<IActionResult> Index()
		{
			var dbLocations = db.Locations
				.Include(l => l.Map)
				.Include(l => l.Cinemas);
			return View(await dbLocations.ToListAsync());
		}

		/* List of locations after being filtered */
		[HttpPost]
		public async Task<IActionResult> Index(string find)
		{
			if (find == null) { return RedirectToAction(nameof(Index)); }
			else
			{
				find = find.Trim();
				var dbLocations = db.Locations
					.Include(l => l.Map)
					.Include(l => l.Cinemas)
					.Where(l => l.City.Contains(find) || l.Address.Contains(find));
				return View(await dbLocations.ToListAsync());
			}
		}

		/* Info of a location */
		public async Task<IActionResult> Info(int? id)
		{
			if (id == null) { return RedirectToAction("Page404", "Home"); }
			var dbLocation = await db.Locations
					.Include(l => l.Map)
					.Include(l => l.Cinemas).ThenInclude(ci => ci.Halls)
					.SingleOrDefaultAsync(l => l.ID == id);
			if (dbLocation == null) { return RedirectToAction("Page404", "Home"); }
			return View(dbLocation);
		}

		/* Create new location */
		public IActionResult Create()
		{
			ViewData["MapID"] = new SelectList(db.Images.Where(i => i.Category == ImageCategory.Maps), "ID", "Name");
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create([Bind("City,Address,Directions,MapID")] Location location)
		{
			LocationDBExist(location.City, location.Address);
			if (ModelState.IsValid)
			{
				try
				{
					db.Locations.Add(location);
					await db.SaveChangesAsync();
					TempData["Status"] = "A new location has been created.";
					TempData["Color"] = "success";
					return RedirectToAction(nameof(Index));
				}
				catch (Exception)
				{
					TempData["Status"] = "We're sorry, an unexpected error has been accured." + Environment.NewLine + "If you keep getting this error please contact system administrator.";
					TempData["Color"] = "danger";
					ViewData["MapID"] = new SelectList(db.Images.Where(i => i.Category == ImageCategory.Maps), "ID", "Name", location.MapID);
					return View(location);
				}				
			}
			ViewData["MapID"] = new SelectList(db.Images.Where(i => i.Category == ImageCategory.Maps), "ID", "Name", location.MapID);
			return View(location);
		}

		
		/* Edit location */
		public async Task<IActionResult> Edit(int? id)
		{
			if (id == null) { return RedirectToAction("Page404", "Home"); }
			var dbLocation = await db.Locations
				.Include(l => l.Map)
				.SingleOrDefaultAsync(l => l.ID == id);
			if (dbLocation == null) { return RedirectToAction("Page404", "Home"); }
			ViewData["MapID"] = new SelectList(db.Images.Where(i => i.Category == ImageCategory.Maps), "ID", "Name", dbLocation.MapID);
			return View(dbLocation);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(int id, [Bind("ID,City,Address,Directions,MapID")] Location location)
		{
			if (id != location.ID)
			{
				TempData["Status"] = "We're sorry, an unexpected error has been accured." + Environment.NewLine + "If you keep getting this error please contact system administrator.";
				TempData["Color"] = "danger";
				return RedirectToAction(nameof(Index));
			}
			LocationDBExist(location.City, location.Address, location.ID);
			if (ModelState.IsValid)
			{
				try
				{
					db.Locations.Update(location);
					await db.SaveChangesAsync();
					TempData["Status"] = "The changes you made to the cinema have been saved!";
					TempData["Color"] = "success";
				}
				catch (DbUpdateConcurrencyException)
				{
					if (!LocationExists(location.ID)) { return RedirectToAction("Page404", "Home"); }
					else
					{
						TempData["Status"] = "We're sorry, an unexpected error has been accured." + Environment.NewLine + "If you keep getting this error please contact system administrator.";
						TempData["Color"] = "danger";
						return RedirectToAction(nameof(Index));
					}
				}
				return RedirectToAction(nameof(Info), new { id = location.ID });
			}
			ViewData["MapID"] = new SelectList(db.Images.Where(i => i.Category == ImageCategory.Maps), "ID", "Name", location.MapID);
			return View(location);
		}


		/* Delete location */
		public async Task<IActionResult> Delete(int? id)
		{
			if (id == null) { return RedirectToAction("Page404", "Home"); }
			var dbLocation = await db.Locations
				.Include(l => l.Map)
				.Include(l => l.Cinemas)
				.SingleOrDefaultAsync(l => l.ID == id);
			if (dbLocation == null) { return RedirectToAction("Page404", "Home"); }
			return View(dbLocation);
		}

		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(int id)
		{
			var dbLocation = await db.Locations
				.SingleOrDefaultAsync(l => l.ID == id);
			if (dbLocation == null) { return RedirectToAction("Page404", "Home"); }
			try
			{
				db.Locations.Remove(dbLocation);
				await db.SaveChangesAsync();
				TempData["Status"] = "The location has been successfully deleted!";
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
		private bool LocationExists(int id)
		{
			return db.Locations.Any(e => e.ID == id);
		}

		/* Server Validations */
		private void LocationDBExist(string cCity, string cAddress, int? id = null)
		{
			// Unique city and address
			var dbLocation = db.Locations.AsNoTracking().FirstOrDefault(l => l.City == cCity && l.Address == cAddress);
			if ((dbLocation != null && id == null) || (dbLocation != null && dbLocation.ID != id))
			{
				ModelState.AddModelError("Address", "A location with that address and city already exists.");
			}
		}

	}
}
