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
	public class ReviewsController : Controller
    {
        private readonly DBContext db;
        public ReviewsController(DBContext context)
        {
            db = context;
        }

		/* List of reviews */
		public async Task<IActionResult> Index()
		{
			var dbReviews = db.Reviews
				.Include(rv => rv.Movie)
				.Include(rv => rv.Client);
			return View(await dbReviews.ToListAsync());
		}

		/* List of cinemas after being filtered */
		[HttpPost]
		public async Task<IActionResult> Index(string find)
		{
			if (find == null) { return RedirectToAction(nameof(Index)); }
			else
			{
				find = find.Trim();
				var dbReviews = db.Reviews
					.Include(rv => rv.Movie)
					.Include(rv => rv.Client)
					.Where(rv => rv.Comment.Contains(find) || rv.Client.UserName.Contains(find) || rv.Movie.Title.Contains(find));
				return View(await dbReviews.ToListAsync());
			}
		}

		/* Info of a review */
		public async Task<IActionResult> Info(int? id)
		{
			if (id == null) { return RedirectToAction("Page404", "Home"); }
			var dbReview = await db.Reviews
				.Include(rv => rv.Movie).ThenInclude(m => m.Poster)
				.Include(rv => rv.Client)
				.SingleOrDefaultAsync(rv => rv.ID == id);
			if (dbReview == null) { return RedirectToAction("Page404", "Home"); }
			return View(dbReview);
		}

		/* Create new review */
		public IActionResult Create()
		{
			ViewData["ClientID"] = new SelectList(db.Clients, "Id", "UserName");
			ViewData["MovieID"] = new SelectList(db.Movies, "ID", "Title");
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create([Bind("Rating,Comment,MovieID,ClientID")] Review review)
		{
			ReviewDBExist(review.ClientID, review.MovieID);
			if (ModelState.IsValid)
			{
				try
				{
					db.Reviews.Add(review);
					await db.SaveChangesAsync();
					TempData["Status"] = "A new review has been added.";
					TempData["Color"] = "success";
					return RedirectToAction(nameof(Index));
				}
				catch (Exception)
				{
					TempData["Status"] = "We're sorry, an unexpected error has been accured." + Environment.NewLine + "If you keep getting this error please contact system administrator.";
					TempData["Color"] = "danger";
					ViewData["ClientID"] = new SelectList(db.Clients, "Id", "UserName", review.ClientID);
					ViewData["MovieID"] = new SelectList(db.Movies, "ID", "Title", review.MovieID);
					return View(review);
				}
			}
			ViewData["ClientID"] = new SelectList(db.Clients, "Id", "UserName", review.ClientID);
			ViewData["MovieID"] = new SelectList(db.Movies, "ID", "Title", review.MovieID);
			return View(review);
		}

		

		/* Edit review */
		public async Task<IActionResult> Edit(int? id)
		{
			if (id == null) { return RedirectToAction("Page404", "Home"); }
			var dbReview = await db.Reviews
				.SingleOrDefaultAsync(rv => rv.ID == id);
			if (dbReview == null) { return RedirectToAction("Page404", "Home"); }
			ViewData["ClientID"] = new SelectList(db.Clients, "Id", "UserName", dbReview.ClientID);
			ViewData["MovieID"] = new SelectList(db.Movies, "ID", "Title", dbReview.MovieID);
			return View(dbReview);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(int id, [Bind("ID,Rating,Comment,MovieID,ClientID")] Review review)
		{
			if (id != review.ID)
			{
				TempData["Status"] = "We're sorry, an unexpected error has been accured." + Environment.NewLine + "If you keep getting this error please contact system administrator.";
				TempData["Color"] = "danger";
				return RedirectToAction(nameof(Index));
			}
			ReviewDBExist(review.ClientID, review.MovieID, review.ID);
			if (ModelState.IsValid)
			{
				try
				{
					db.Reviews.Update(review);
					await db.SaveChangesAsync();
					TempData["Status"] = "The changes you made to the review have been saved!";
					TempData["Color"] = "success";
				}
				catch (DbUpdateConcurrencyException)
				{
					if (!ReviewExists(review.ID)) { return RedirectToAction("Page404", "Home"); }
					TempData["Status"] = "We're sorry, an unexpected error has been accured." + Environment.NewLine + "If you keep getting this error please contact system administrator.";
					TempData["Color"] = "danger";
					return RedirectToAction(nameof(Index));
				}
				return RedirectToAction(nameof(Info), new { id = review.ID });
			}
			ViewData["ClientID"] = new SelectList(db.Clients, "Id", "UserName", review.ClientID);
			ViewData["MovieID"] = new SelectList(db.Movies, "ID", "Title", review.MovieID);
			return View(review);
		}


		/* Delete review */
		public async Task<IActionResult> Delete(int? id)
		{
			if (id == null) { return RedirectToAction("Page404", "Home"); }
			var dbReview = await db.Reviews
				.Include(rv => rv.Movie)
				.Include(rv => rv.Client)
				.SingleOrDefaultAsync(rv => rv.ID == id);
			if (dbReview == null) { return RedirectToAction("Page404", "Home"); }
			return View(dbReview);
		}

		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(int id)
		{
			var dbReview = await db.Reviews
				.SingleOrDefaultAsync(rv => rv.ID == id);
			if (dbReview == null) { return RedirectToAction("Page404", "Home"); }
			try
			{
				db.Reviews.Remove(dbReview);
				await db.SaveChangesAsync();
				TempData["Status"] = "The review has been successfully deleted!";
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
		private bool ReviewExists(int id)
        {
            return db.Reviews.Any(e => e.ID == id);
        }

		/* Server Validations */
		private void ReviewDBExist(string clientID, int movieID, int? id = null)
		{
			// Unique client and movie - only 1 review for client to the movie
			var dbReview = db.Reviews.AsNoTracking().FirstOrDefault(rv => rv.ClientID == clientID && rv.MovieID == movieID);
			if ((dbReview != null && id == null) || (dbReview != null && dbReview.ID != id))
			{
				ModelState.AddModelError("ClientID", "A review to that movie by this client already exist.");
			}
		}
	}
}
