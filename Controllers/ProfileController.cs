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
    public class ProfileController : Controller
    {
		private readonly DBContext db;
		private readonly UserManager<Client> userManager;
		private readonly SignInManager<Client> signInManager;

		public ProfileController(DBContext context, UserManager<Client> uManager, SignInManager<Client> sManager)
		{
			db = context;
			userManager = uManager;
			signInManager = sManager;
		}

		[HttpGet]
		public async Task<IActionResult> Index()
		{
			var clientID = userManager.GetUserId(User);
			var dbClient = await db.Clients
				.Include(cl => cl.Avatar)
				.Include(cl => cl.Orders)
				.Include(cl => cl.Reviews)
				.SingleOrDefaultAsync(cl => cl.Id == clientID);
			if (dbClient == null) { return NotFound(); }
			return View(dbClient);
		}


		[HttpGet]
		public async Task<IActionResult> Edit()
		{
			var user = await userManager.GetUserAsync(User);
			if (user == null) { return NotFound(); }
			var model = new EditProfileViewModel
			{
				Username = user.UserName,
				Email = user.Email,
				AvatarID = user.AvatarID
			};
			model.Avatar = await db.Images.SingleOrDefaultAsync(i => i.ID == model.AvatarID);
			ViewData["AvatarID"] = new SelectList(db.Images.Where(i => i.Category == ImageCategory.Avatars), "ID", "Name", model.AvatarID);
			return View(model);

			//var model = new Models.ManageViewModels.IndexViewModel
			//{
			//	Username = user.UserName,
			//	Email = user.Email,
			//	PhoneNumber = user.PhoneNumber,
			//	IsEmailConfirmed = user.EmailConfirmed,
			//	StatusMessage = StatusMessage
			//};

			//return View(model);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit([Bind("AvatarID,Username,Email")] EditProfileViewModel model)
		{
			if (!ModelState.IsValid)
			{
				model.Avatar = await db.Images.SingleOrDefaultAsync(i => i.ID == model.AvatarID);
				return View(model);
			}

			var user = await userManager.GetUserAsync(User);
			if (user == null) { return NotFound(); }

			var email = user.Email;
			if (model.Email != email)
			{
				var setEmailResult = await userManager.SetEmailAsync(user, model.Email);
				if (!setEmailResult.Succeeded) { return NotFound(); }
				//{
				//	throw new ApplicationException($"Unexpected error occurred setting email for user with ID '{user.Id}'.");
				//}
			}

			var userName = user.UserName;
			if (model.Username != userName)
			{
				var setUserResult = await userManager.SetUserNameAsync(user, model.Username);
				if (!setUserResult.Succeeded) { return NotFound(); }
				//{
				//	throw new ApplicationException($"Unexpected error occurred setting phone number for user with ID '{user.Id}'.");
				//}
			}

			var userAvatar = user.AvatarID;
			if (model.AvatarID != userAvatar)
			{
				user.AvatarID = model.AvatarID;
				db.Clients.Update(user);
				db.SaveChanges();
			}

			//StatusMessage = "Your profile has been updated";
			return RedirectToAction(nameof(Index));

		}


		[HttpGet]
		public async Task<IActionResult> ChangePassword()
		{
			var user = await userManager.GetUserAsync(User);
			if (user == null) { return NotFound(); }
			//{
			//	throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
			//}

			var model = new ProfilePassViewModel();
			return View(model);
			//var hasPassword = await _userManager.HasPasswordAsync(user);
			//if (!hasPassword)
			//{
			//	return RedirectToAction(nameof(SetPassword));
			//}

			//var model = new ChangePasswordViewModel { StatusMessage = StatusMessage };
			//return View(model);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> ChangePassword([Bind("OldPassword,NewPassword,ConfirmPassword")] ProfilePassViewModel model)
		{
			if (!ModelState.IsValid)
			{
				return View(model);
			}

			var user = await userManager.GetUserAsync(User);
			if (user == null) { return NotFound(); }
			//{
			//	throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
			//}

			var changePassResult = await userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
			if (!changePassResult.Succeeded)
			{
				AddErrors(changePassResult);
				return View(model);
			}

			await signInManager.SignInAsync(user, isPersistent: true);
			//_logger.LogInformation("User changed their password successfully.");
			//StatusMessage = "Your password has been changed.";

			return RedirectToAction(nameof(Index));
		}

		private void AddErrors(IdentityResult result)
		{
			foreach (var error in result.Errors)
			{
				ModelState.AddModelError(string.Empty, error.Description);
			}
		}


		public async Task<IActionResult> MyOrders()
		{
			var clientID = userManager.GetUserId(User);
			var dbOrders = db.Orders
				.Include(order => order.Client)
				.Include(order => order.Tickets).ThenInclude(ticket => ticket.Show).ThenInclude(show => show.Hall).ThenInclude(h => h.Cinema)
				.Include(order => order.Tickets).ThenInclude(ticket => ticket.Show).ThenInclude(show => show.Movie)
				.Where(o => o.ClientID == clientID)
				.OrderByDescending(o => o.OrderTime);
			return View(await dbOrders.ToListAsync());
		}


		public async Task<IActionResult> MyReviews()
		{
			var clientID = userManager.GetUserId(User);
			var dbReviews = db.Reviews
				.Include(rv => rv.Movie)
				.Include(rv => rv.Client)
				.Where(rv => rv.ClientID == clientID)
				.OrderByDescending(rv => rv.PostTime);
			return View(await dbReviews.ToListAsync());
		}

		[AllowAnonymous]
		[HttpPost]
		public JsonResult GetAvatar(int id)
		{
			var dbImage = db.Images
				.SingleOrDefault(i => i.ID == id);
			if (dbImage == null || dbImage.Category != ImageCategory.Avatars)
			{
				Response.StatusCode = 404;
				return Json(Response.StatusCode);
			}
			return Json(new { File = dbImage.FilePath });
		}

    }
}
