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
	public class OrdersController : Controller
    {
        private readonly DBContext db;
        public OrdersController(DBContext context)
        {
            db = context;
        }

        // GET: Admin/Orders
        public async Task<IActionResult> Index()
        {
			var dbOrders = db.Orders
				.Include(order => order.Client)
				.Include(order => order.Tickets).ThenInclude(ticket => ticket.Show).ThenInclude(show => show.Hall).ThenInclude(h => h.Cinema)
				.Include(order => order.Tickets).ThenInclude(ticket => ticket.Show).ThenInclude(show => show.Movie);
			return View(await dbOrders.ToListAsync());
        }

		/* List of orders after being filtered */
		[HttpPost]
		public async Task<IActionResult> Index(string find)
		{
			if (find == null) { return RedirectToAction(nameof(Index)); }
			else
			{
				find = find.Trim();
				var dbOrders = db.Orders
				.Include(order => order.Client)
				.Include(order => order.Tickets).ThenInclude(ticket => ticket.Show).ThenInclude(show => show.Hall).ThenInclude(h => h.Cinema)
				.Include(order => order.Tickets).ThenInclude(ticket => ticket.Show).ThenInclude(show => show.Movie)
				.Where(o => o.Client.UserName.Contains(find) || o.Client.Email.Contains(find));
				return View(await dbOrders.ToListAsync());
			}
		}

		// GET: Admin/Orders/Details/5
		public async Task<IActionResult> Info(int? id)
        {
			if (id == null) { return RedirectToAction("Page404", "Home"); }
			var dbOrder = await db.Orders
				.Include(o => o.Payment)
				.Include(order => order.Client)
				.Include(order => order.Tickets).ThenInclude(ticket => ticket.Show).ThenInclude(show => show.Hall).ThenInclude(h => h.Cinema)
				.Include(order => order.Tickets).ThenInclude(ticket => ticket.Show).ThenInclude(show => show.Movie)
				.SingleOrDefaultAsync(order => order.ID == id);
			if (dbOrder == null) { return RedirectToAction("Page404", "Home"); }
			return View(dbOrder);
		}

		/* Delete order */
		public async Task<IActionResult> Delete(int? id)
		{
			if (id == null) { return RedirectToAction("Page404", "Home"); }
			var dbOrder = await db.Orders
				.Include(o => o.Payment)
				.Include(order => order.Client)
				.Include(order => order.Tickets).ThenInclude(ticket => ticket.Show).ThenInclude(show => show.Hall).ThenInclude(h => h.Cinema)
				.Include(order => order.Tickets).ThenInclude(ticket => ticket.Show).ThenInclude(show => show.Movie)
				.SingleOrDefaultAsync(order => order.ID == id);
			if (dbOrder == null) { return RedirectToAction("Page404", "Home"); }
			return View(dbOrder);
		}

		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(int id)
		{
			var dbOrder = await db.Orders
				.SingleOrDefaultAsync(o => o.ID == id);
			if (dbOrder == null) { return RedirectToAction("Page404", "Home"); }
			try
			{
				db.Orders.Remove(dbOrder);
				await db.SaveChangesAsync();
				TempData["Status"] = "The order has been successfully deleted!";
				TempData["Color"] = "success";
			}
			catch (Exception)
			{
				TempData["Status"] = "We're sorry, an unexpected error has been accured." + Environment.NewLine + "If you keep getting this error please contact system administrator.";
				TempData["Color"] = "danger";
			}			
			return RedirectToAction(nameof(Index));
		}

		/* Server Validations */
		//private void OrderDBExist(string cName, int? id = null)
		//{
		//	// Unique name
		//	var dbCinema = db.Cinemas.AsNoTracking().FirstOrDefault(ci => ci.Name == cName);
		//	if ((dbCinema != null && id == null) || (dbCinema != null && dbCinema.ID != id))
		//	{
		//		ModelState.AddModelError("Name", "A cinema with that name already exists.");
		//	}
		//}
	}
}
