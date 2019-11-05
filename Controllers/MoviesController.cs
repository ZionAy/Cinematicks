using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Cinematicks.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace Cinematicks.Controllers
{
	[AllowAnonymous]
    public class MoviesController : Controller
    {
		private readonly DBContext db;
		private readonly UserManager<Client> userManager;
		public MoviesController(DBContext context, UserManager<Client> uManager)
        {
            db = context;
			userManager = uManager;
        }

		/* All movies */
		public async Task<IActionResult> Index()
		{
			var dbMovies = db.Movies
				.Include(m => m.Genres).ThenInclude(mg => mg.Genre)
				.Include(m => m.Rate).ThenInclude(rt => rt.Image)
				.Include(m => m.Poster)
				.Include(m => m.Reviews)
				.Where(m => m.Shows.Any(sh => sh.ShowDate > DateTime.Today || (sh.ShowDate == DateTime.Today && sh.ShowTime > DateTime.Now.TimeOfDay)));
			ViewData["Category"] = "All Movies";
			return View(await dbMovies.ToListAsync());
		}

		/* Kids movies */
		public async Task<IActionResult> Kids()
		{
			var dbMovies = db.Movies
				.Include(m => m.Genres).ThenInclude(mg => mg.Genre)
				.Include(m => m.Rate).ThenInclude(rt => rt.Image)
				.Include(m => m.Poster)
				.Include(m => m.Reviews)
				.Where(m => m.Shows.Any(sh => sh.ShowDate > DateTime.Today || (sh.ShowDate == DateTime.Today && sh.ShowTime > DateTime.Now.TimeOfDay)))
				.Where(m => m.Rate.MinAge < 13);
			ViewData["Category"] = "Kids Movies";
			return View(await dbMovies.ToListAsync());
		}

		/* Coming soon movies */
		public async Task<IActionResult> ComingSoon()
		{
			var dbMovies = db.Movies
				.Include(m => m.Genres).ThenInclude(mg => mg.Genre)
				.Include(m => m.Rate).ThenInclude(rt => rt.Image)
				.Include(m => m.Poster)
				.Include(m => m.Reviews)
				.Where(m => m.Release > DateTime.Today);
			ViewData["Category"] = "Coming Soon Movies";
			return View(await dbMovies.ToListAsync());
		}

		/* Movies by genre */
		public async Task<IActionResult> Genre(int? id)
		{
			// No such genre -> show all movies page
			if (id == null) { return RedirectToAction(nameof(Index)); }
			var dbGenre = db.Genres.SingleOrDefault(g => g.ID == id);
			if (dbGenre == null) { return RedirectToAction(nameof(Index)); }

			var dbMovies = db.Movies
				.Include(m => m.Genres).ThenInclude(mg => mg.Genre)
				.Include(m => m.Rate).ThenInclude(rt => rt.Image)
				.Include(m => m.Poster)
				.Include(m => m.Reviews)
				.Where(m => m.Shows.Any(sh => sh.ShowDate > DateTime.Today || (sh.ShowDate == DateTime.Today && sh.ShowTime > DateTime.Now.TimeOfDay)))
				.Where(m => m.Genres.Any(mg => mg.GenreID == id));
			ViewData["Category"] = dbGenre.Name + " Movies";
			return View(await dbMovies.ToListAsync());
		}

		/* Movie info page */
		public async Task<IActionResult> Movie(int? id)
		{
			if (id == null) { return RedirectToAction("Page404", "Home"); }
			var dbMovie = await db.Movies
				.Include(m => m.Genres).ThenInclude(mg => mg.Genre)
				.Include(m => m.Rate).ThenInclude(rt => rt.Image)
				.Include(m => m.Poster)
				.Include(m => m.Reviews).ThenInclude(rv => rv.Client).ThenInclude(cl => cl.Avatar)
				.SingleOrDefaultAsync(m => m.ID == id);
			if (dbMovie == null) { return RedirectToAction("Page404", "Home"); }
			return View(dbMovie);
		}

		/* Add review to movie */
		[Authorize]
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> AddReview([Bind("Rating,Comment,MovieID,ClientID")] Review review)
		{
			ReviewDBExist(review.ClientID, review.MovieID);
			if (ModelState.IsValid)
			{
				try
				{
					db.Reviews.Add(review);
					await db.SaveChangesAsync();
					TempData["Status"] = "Your review has been added!";
					TempData["Color"] = "success";
				}
				catch (Exception)
				{
					TempData["Status"] = "Something went wrong, please try again!";
					TempData["Color"] = "danger";
				}
			}
			return RedirectToAction(nameof(Movie), "Movies" , new { id = review.MovieID }, fragment: "movie-reviews");
		}


		/* Server Validations */
		private void ReviewDBExist(string clientID, int movieID, int? id = null)
		{
			// Unique client and movie - only 1 review for client to the movie
			var dbReview = db.Reviews.AsNoTracking().FirstOrDefault(rv => rv.ClientID == clientID && rv.MovieID == movieID);
			if ((dbReview != null && id == null) || (dbReview != null && dbReview.ID != id))
			{
				TempData["Status"] = "You already reviewed this movie!";
				TempData["Color"] = "danger";
				RedirectToAction(nameof(Movie), "Movies", new { id = movieID }, fragment: "movie-reviews");
			}
		}
	}
}
