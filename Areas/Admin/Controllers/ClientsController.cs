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

namespace Cinematicks.Areas.Admin.Controllers
{
    [Area("Admin")]
	[Authorize(Roles = "Admin")]
	public class ClientsController : Controller
    {
		private readonly UserManager<Client> UserManager;
		private readonly RoleManager<IdentityRole> RoleManager;
		private readonly DBContext db;
        public ClientsController(UserManager<Client> uManager, RoleManager<IdentityRole> rManager, DBContext context)
        {
			UserManager = uManager;
			RoleManager = rManager;
			db = context;
        }

		/* List of clients */
		public async Task<IActionResult> Index()
		{
			var dbClients = db.Clients
				.Include(cl => cl.Avatar)
				.Include(cl => cl.Orders)
				.Include(cl => cl.Reviews);
			return View(await dbClients.ToListAsync());
		}

		/* List of clients after being filtered */
		[HttpPost]
		public async Task<IActionResult> Index(string find)
		{
			if (find == null) { return RedirectToAction(nameof(Index)); }
			else
			{
				find = find.Trim();
				var dbClients = db.Clients
					.Include(cl => cl.Avatar)
					.Include(cl => cl.Orders)
					.Include(cl => cl.Reviews)
					.Where(cl => cl.UserName.Contains(find) || cl.Email.Contains(find));
				return View(await dbClients.ToListAsync());
			}
		}

		/* Info of a client */
		public async Task<IActionResult> Info(string id)
		{
			if (id == null) { return RedirectToAction("Page404", "Home"); }
			var dbClient = await db.Clients
				.Include(cl => cl.Avatar)
				.Include(cl => cl.Orders).ThenInclude(o => o.Tickets)
				.Include(cl => cl.Reviews).ThenInclude(rv => rv.Movie)
				.SingleOrDefaultAsync(cl => cl.Id == id);
			if (dbClient == null) { return RedirectToAction("Page404", "Home"); }
			return View(dbClient);
		}

		/* Create new cinema */
		public IActionResult Create()
		{
			ViewData["AvatarID"] = new SelectList(db.Images.Where(i => i.Category == ImageCategory.Avatars), "ID", "Name", db.Images.SingleOrDefault(i => i.Category == ImageCategory.Avatars && i.Name == "No Avatar").ID);
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create([Bind("UserName,Email,Password,ConfirmPassword,IsAdmin,AvatarID")] NewClientViewModel model)
		{
			ClientDBExist(model.UserName, model.Email);
			if (ModelState.IsValid)
			{
				var user = new Client
				{
					UserName = model.UserName,
					Email = model.Email,
					AvatarID = model.AvatarID,
					EmailConfirmed = true,
					RegisterTime = DateTime.Now,
					LockoutEnabled = true
				};
				var result = await UserManager.CreateAsync(user, model.Password);
				if (result.Succeeded)
				{
					if (model.IsAdmin)
					{
						var resultAdmin = await UserManager.AddToRoleAsync(user, "Admin");
					}
					TempData["Status"] = "A client named " + model.UserName + " has been created.";
					TempData["Color"] = "success";
					return RedirectToAction(nameof(Index));
				}
				else
				{
					var error = result.Errors.FirstOrDefault();
					if (error.Code == "InvalidUserName")
					{
						ModelState.AddModelError("UserName", "UserName must contains only letters, digits, - or _ characters.");
					}
					else
					{
						ModelState.AddModelError("Password", "Password must be 6-16 characters long and have at least 1 letter and 1 digit.");
					}
				}
			}
			ViewData["AvatarID"] = new SelectList(db.Images.Where(i => i.Category == ImageCategory.Avatars), "ID", "Name", model.AvatarID);
			return View(model);
		}

		/* Edit client */
		public async Task<IActionResult> Edit(string id)
		{
			if (id == null) { return RedirectToAction("Page404", "Home"); }
			var dbClient = await db.Clients
				.SingleOrDefaultAsync(cl => cl.Id == id);
			if (dbClient == null) { return RedirectToAction("Page404", "Home"); }
			var model = new EditClientViewModel()
			{
				ID = dbClient.Id,
				OriginalUserName = dbClient.UserName,
				UserName = dbClient.UserName,
				Email = dbClient.Email,
				AvatarID = dbClient.AvatarID,
				IsAdmin = await UserManager.IsInRoleAsync(dbClient, "Admin")
			};
			ViewData["AvatarID"] = new SelectList(db.Images.Where(i => i.Category == ImageCategory.Avatars), "ID", "Name", model.AvatarID);
			return View(model);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(string id, [Bind("ID,OriginalUserName,UserName,Email,Password,ConfirmPassword,IsAdmin,AvatarID")] EditClientViewModel client)
		{
			if (id != client.ID)
			{
				TempData["Status"] = "We're sorry, an unexpected error has been accured." + Environment.NewLine + "If you keep getting this error please contact system administrator.";
				TempData["Color"] = "danger";
				return RedirectToAction(nameof(Index));
			}
			ClientDBExist(client.UserName, client.Email, client.ID);
			if (ModelState.IsValid)
			{
				try
				{
					var dbClient = db.Clients
						.SingleOrDefault(cl => cl.Id == id);
					if (dbClient == null)
					{
						TempData["Status"] = "We're sorry, the client you are trying to edit seems to be deleted.";
						TempData["Color"] = "danger";
						return RedirectToAction(nameof(Index));
					}

					var username = dbClient.UserName;
					if (client.UserName != username)
					{
						var setUserNameResult = await UserManager.SetUserNameAsync(dbClient, client.UserName);
						if (!setUserNameResult.Succeeded)
						{
							TempData["Status"] = "We're sorry, an unexpected error has been accured." + Environment.NewLine + "If you keep getting this error please contact system administrator.";
							TempData["Color"] = "danger";
							return RedirectToAction(nameof(Index));
						}
					}

					var email = dbClient.Email;
					if (client.Email != email)
					{
						var setEmailResult = await UserManager.SetEmailAsync(dbClient, client.Email);
						if (!setEmailResult.Succeeded)
						{
							TempData["Status"] = "We're sorry, an unexpected error has been accured." + Environment.NewLine + "If you keep getting this error please contact system administrator.";
							TempData["Color"] = "danger";
							return RedirectToAction(nameof(Index));
						}
					}

					if (client.Password != null)
					{
						var newPass = UserManager.PasswordHasher.HashPassword(dbClient, client.Password);
						if (newPass != null)
						{
							dbClient.PasswordHash = newPass;
						}
					}

					if (client.AvatarID != dbClient.AvatarID)
					{
						dbClient.AvatarID = client.AvatarID;
					}

					var checkAdmin = await UserManager.IsInRoleAsync(dbClient, "Admin");
					if (client.IsAdmin != checkAdmin)
					{
						IdentityResult resultAdmin;
						if (client.IsAdmin)
						{
							resultAdmin = await UserManager.AddToRoleAsync(dbClient, "Admin");
						}
						else
						{
							resultAdmin = await UserManager.RemoveFromRoleAsync(dbClient, "Admin");
						}

						if (!resultAdmin.Succeeded)
						{
							TempData["Status"] = "We're sorry, an unexpected error has been accured." + Environment.NewLine + "If you keep getting this error please contact system administrator.";
							TempData["Color"] = "danger";
							return RedirectToAction(nameof(Index));
						}
					}
					db.Clients.Update(dbClient);
					await db.SaveChangesAsync();
					TempData["Status"] = "The changes you made to the client have been saved!";
					TempData["Color"] = "success";
				}
				catch (DbUpdateConcurrencyException)
				{
					if (!ClientExists(client.ID)) { return RedirectToAction("Page404", "Home"); }
					TempData["Status"] = "We're sorry, an unexpected error has been accured." + Environment.NewLine + "If you keep getting this error please contact system administrator.";
					TempData["Color"] = "danger";
					return RedirectToAction(nameof(Index));
				}
				return RedirectToAction(nameof(Info), new { id = client.ID });
			}
			ViewData["AvatarID"] = new SelectList(db.Images.Where(i => i.Category == ImageCategory.Avatars), "ID", "Name", client.AvatarID);
			return View(client);
		}


		/* Delete client */
		public async Task<IActionResult> Delete(string id)
		{
			if (id == null || id == "" || id == " ") { return RedirectToAction("Page404", "Home"); }
			var dbClient = await db.Clients
				.Include(cl => cl.Avatar)
				.Include(cl => cl.Orders)
				.Include(cl => cl.Reviews)
				.SingleOrDefaultAsync(cl => cl.Id == id);
			if (dbClient == null) { return RedirectToAction("Page404", "Home"); }
			return View(dbClient);
		}

		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(string id)
		{
			var dbClient = await db.Clients
				.SingleOrDefaultAsync(cl => cl.Id == id);
			var deleteResult = await UserManager.DeleteAsync(dbClient);
			if (!deleteResult.Succeeded)
			{
				TempData["Status"] = "We're sorry, an unexpected error has been accured." + Environment.NewLine + "If you keep getting this error please contact system administrator.";
				TempData["Color"] = "danger";
				return RedirectToAction(nameof(Index));
			}
			TempData["Status"] = "The client has been successfully deleted!";
			TempData["Color"] = "success";
			return RedirectToAction(nameof(Index));
		}

        private bool ClientExists(string id)
        {
            return db.Clients.Any(e => e.Id == id);
        }

		/* Server Validations */
		private void ClientDBExist(string cUser, string cEmail, string id = null)
		{
			// Unique username
			var dbClientUser = db.Clients.AsNoTracking().FirstOrDefault(cl => cl.UserName.ToUpper() == cUser.ToUpper());
			if ((dbClientUser != null && id == null) || (dbClientUser != null && dbClientUser.Id != id))
			{
				ModelState.AddModelError("UserName", "A client with this username already exists.");
			}
			// Unique email
			var dbClientEmail = db.Clients.AsNoTracking().FirstOrDefault(cl => cl.Email.ToUpper() == cEmail.ToUpper());
			if ((dbClientEmail != null && id == null) || (dbClientEmail != null && dbClientEmail.Id != id))
			{
				ModelState.AddModelError("Email", "A client with this email already exists.");
			}
		}

	}
}
