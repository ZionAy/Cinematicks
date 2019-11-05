using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Cinematicks.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Cinematicks.ViewModels;

namespace Cinematicks.Controllers
{
	[Authorize]
	public class OrderController : Controller
    {
		private readonly DBContext db;
		private readonly UserManager<Client> userManager;
		public OrderController(DBContext context, UserManager<Client> uManager)
		{
			db = context;
			userManager = uManager;
		}

		/* New order */
		public IActionResult Index(int? movieID = null)
		{
			var model = new NewOrderViewModel(db, movieID);
			ViewData["Movies"] = new SelectList(model.MoviesList, "ID", "Title");
			ViewData["Dates"] = new SelectList(model.DatesList, "Value", "Text");
			return View(model);
		}

		public ActionResult ShowsTable(int movieID, string date)
		{
			DateTime fDateID;
			fDateID = DateTime.TryParse(date, out fDateID) ? fDateID : DateTime.Today;
			var model = new OrderShowsViewModel(db, movieID, fDateID);
			return PartialView("_ShowsTable", model);
		}

		public ActionResult New(int? id)
		{
			if (id == null) { return RedirectToAction("Page404", "Home"); }
			var model = new MakeOrderViewModel(db, id.Value);
			return View(model);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> New(int showID, string[] Seats, [Bind("FirstName,LastName,CCID,CCNum,CCCVV,CCMonth,CCYear,Address,City,ZipCode,SendInvoice")] Payment payment, string client)
		{
			string user = "";
			if (client != null)
			{
				Client adminChoise = await userManager.FindByEmailAsync(client);
				user = adminChoise.Id;
			}
			else
			{
				user = userManager.GetUserId(User);
			}
			var ticketsList = CreateTickets(showID, user, Seats);
			if (ticketsList == null) { return NotFound(); }
			var order = new Order
			{
				ClientID = user,
				Payment = payment,
				Tickets = ticketsList
			};
			try
			{
				db.Orders.Add(order);
				db.SaveChanges();
				TempData["Status"] = "Your order has been recieved.";
				TempData["Color"] = "success";
				if (client != null)
				{
					return RedirectToAction("Info", "Orders", new { id = order.ID, area = "Admin" });
				}

				return RedirectToAction(nameof(Invoice), new { id = order.ID });
			}
			catch (Exception)
			{
				TempData["Status"] = "Someone has already taken those seats, try to choose again.";
				TempData["Color"] = "danger";
				return RedirectToAction(nameof(New), new { id = showID });
			}
			
		}

		public ActionResult Invoice(int? id)
		{
			if (id == null) { return RedirectToAction("Page404", "Home"); }
			var dbOrder = db.Orders
				.Include(o => o.Payment)
				.Include(order => order.Client)
				.Include(order => order.Tickets).ThenInclude(ticket => ticket.Show).ThenInclude(show => show.Hall).ThenInclude(h => h.Cinema)
				.Include(order => order.Tickets).ThenInclude(ticket => ticket.Show).ThenInclude(show => show.Movie)
				.SingleOrDefault(order => order.ID == id);
			if (dbOrder == null || userManager.GetUserId(User) != dbOrder.ClientID) { return RedirectToAction("Page404", "Home"); }
			return View(dbOrder);
		}

		/* Create ticket list for the order */
		private List<Ticket> CreateTickets(int showId, string user, string[] seats)
		{
			if (seats.Count() == 0) { return null; }
			var ticketsList = new List<Ticket>();
			foreach (var seat in seats)
			{
				var rowCol = seat.Split('-');
				//if row or col have wrong data type
				if (!int.TryParse(rowCol[0], out int tempRow)) { return null; }
				if (!int.TryParse(rowCol[1], out int tempCol)) { return null; }
				var dbTicket = db.Tickets
						.SingleOrDefault(ticket => ticket.ShowID == showId && ticket.Row == tempRow && ticket.Col == tempCol);
				if (dbTicket != null) { return null; }
				ticketsList.Add(new Ticket
				{
					ClientID = user,
					ShowID = showId,
					Row = tempRow,
					Col = tempCol
				});
			}
			return ticketsList;
		}

    }
}
