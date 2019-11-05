using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Cinematicks.Models;
using Cinematicks.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Cinematicks.Services;

namespace Cinematicks.Controllers
{
	[AllowAnonymous]
	[Route("[action]")]
	public class HomeController : Controller
	{
		private readonly DBContext db;
		private readonly IEmailSender emailSender;
		public HomeController(DBContext context, IEmailSender eSender)
		{
			db = context;
			emailSender = eSender;
		}

		[Route("")]
		[Route("/")]
		public IActionResult Index()
		{
			var model = new IndexViewModel(db);
			return View(model);
		}

		public IActionResult Info()
		{
			var model = new InfoViewModel(db);
			return View(model);
		}

		public async Task<IActionResult> Promos()
		{
			var model = await db.Promos
				.Include(pr => pr.Banner)
				.Where(pr => pr.EndTime >= DateTime.Today)
				.OrderByDescending(pr => pr.StartTime)
				.ToListAsync();
			return View(model);
		}

		public IActionResult Contact()
		{
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Contact([Bind("Fullname,Phone,Email,Message")] ContactViewModel model)
		{
			if (ModelState.IsValid)
			{
				string emailBody = $"You have recieved a message from the contact page on the site.<br>";
				emailBody += $"<strong>Name: </strong>{model.Fullname}.<br>";
				emailBody += $"<strong>Email: </strong>{model.Email}.<br>";
				emailBody += $"<strong>Phone: </strong>{model.Phone}<br>";
				emailBody += $"<strong>Message: </strong>{model.Message}";

				await emailSender.SendEmailAsync("cinematicks1@gmail.com", "Someone contacted you from the site", emailBody);
				TempData["Status"] = "Your message was sent to the cinema." + Environment.NewLine + "One of our staff members will contact you soon.";
				TempData["Color"] = "success";
				return RedirectToAction(nameof(Contact));
			}
			return View(model);
		}

		public IActionResult FAQ()
		{
			return View();
		}

		// Custom not found page
		public IActionResult Page404()
		{
			return View();
		}

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
