using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Cinematicks.Models;
using Microsoft.AspNetCore.Authorization;
using Cinematicks.ViewModels;

namespace Cinematicks.Areas.Admin.Controllers
{
    [Area("Admin")]
	[Authorize(Roles = "Admin")]
	public class MoviesController : Controller
    {
        private readonly DBContext db;
        public MoviesController(DBContext context)
        {
            db = context;
        }

		/* List of movies */
		public async Task<IActionResult> Index()
		{
			var dbMovies = db.Movies
				.Include(m => m.Poster)
				.Include(m => m.Rate)
				.Include(m => m.Reviews);
			return View(await dbMovies.ToListAsync());
		}

		/* List of cinemas after being filtered */
		[HttpPost]
		public async Task<IActionResult> Index(string find)
		{
			if (find == null) { return RedirectToAction(nameof(Index)); }
			else
			{
				find = find.Trim();
				var dbMovies = db.Movies
					.Include(m => m.Poster)
					.Include(m => m.Rate)
					.Include(m => m.Reviews)
					.Where(m => m.Title.Contains(find));
				return View(await dbMovies.ToListAsync());
			}
		}

		/* Info of a movie */
		public async Task<IActionResult> Info(int? id)
		{
			if (id == null) { return RedirectToAction("Page404", "Home"); }
			var dbMovie = await db.Movies
				.Include(m => m.Genres).ThenInclude(mg => mg.Genre)
				.Include(m => m.Poster)
				.Include(m => m.Rate)
				.Include(m => m.Reviews).ThenInclude(rv => rv.Client)
				.SingleOrDefaultAsync(m => m.ID == id);
			if (dbMovie == null) { return RedirectToAction("Page404", "Home"); }
			return View(dbMovie);
		}

		/* Create new movie */
		public IActionResult Create()
		{
			var model = new NewMovieViewModel(db);
			ViewData["PosterID"] = new SelectList(db.Images.Where(i => i.Category == ImageCategory.Posters), "ID", "Name");
			ViewData["RateID"] = new SelectList(db.Rates, "ID", "Code");
			return View(model);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create([Bind("Title,Release,Time,RateID,IsDub,Director,Actors,Plot,Trailer,PosterID")] Movie movie, int[] GenreList)
		{
			MovieDBExist(movie.Title, movie.Trailer);
			if (GenreList.Count() == 0)
			{
				ModelState.AddModelError("Genres", "At least one genre need to be selected.");
			}
			if (ModelState.IsValid)
			{
				try
				{
					db.Movies.Add(movie);
					foreach (int item in GenreList)
					{
						db.MovGens.Add(new MovGen { MovieID = movie.ID, GenreID = item });
					}
					await db.SaveChangesAsync();
					TempData["Status"] = "A new movie has been added.";
					TempData["Color"] = "success";
					return RedirectToAction(nameof(Index));
				}
				catch (Exception)
				{
					TempData["Status"] = "We're sorry, an unexpected error has been accured." + Environment.NewLine + "If you keep getting this error please contact system administrator.";
					TempData["Color"] = "danger";
					var erModel = new NewMovieViewModel(db);
					erModel.Movie = movie;
					erModel.Genres = erModel.EditGenres(GenreList);
					ViewData["PosterID"] = new SelectList(db.Images.Where(i => i.Category == ImageCategory.Posters), "ID", "Name", movie.PosterID);
					ViewData["RateID"] = new SelectList(db.Rates, "ID", "Code", movie.RateID);
					return View(erModel);
				}
				
			}
			var model = new NewMovieViewModel(db);
			model.Movie = movie;
			model.Genres = model.EditGenres(GenreList);
			ViewData["PosterID"] = new SelectList(db.Images.Where(i => i.Category == ImageCategory.Posters), "ID", "Name", movie.PosterID);
			ViewData["RateID"] = new SelectList(db.Rates, "ID", "Code", movie.RateID);
			return View(model);
		}


		/* Edit movie */
		public async Task<IActionResult> Edit(int? id)
		{
			if (id == null) { return RedirectToAction("Page404", "Home"); }
			var dbMovie = await db.Movies
				.SingleOrDefaultAsync(m => m.ID == id);
			if (dbMovie == null) { return RedirectToAction("Page404", "Home"); }
			var model = new NewMovieViewModel(db, id);
			ViewData["PosterID"] = new SelectList(db.Images.Where(i => i.Category == ImageCategory.Posters), "ID", "Name", dbMovie.PosterID);
			ViewData["RateID"] = new SelectList(db.Rates, "ID", "Code", dbMovie.RateID);
			return View(model);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(int id, [Bind("ID,Title,Release,Time,RateID,IsDub,Director,Actors,Plot,Trailer,PosterID")] Movie movie, int[] GenreList)
		{
			if (id != movie.ID)
			{
				TempData["Status"] = "We're sorry, an unexpected error has been accured." + Environment.NewLine + "If you keep getting this error please contact system administrator.";
				TempData["Color"] = "danger";
				return RedirectToAction(nameof(Index));
			}
			MovieDBExist(movie.Title, movie.Trailer, movie.ID);
			if (GenreList.Count() == 0)
			{
				ModelState.AddModelError("Genres", "At least one genre need to be selected.");
			}
			if (ModelState.IsValid)
			{
				try
				{
					db.Movies.Update(movie);
					//Edit the genres
					var selectedGenres = GenreList.ToHashSet();
					var movieGenres = db.MovGens.Where(mg => mg.MovieID == movie.ID).Select(g => g.GenreID).ToHashSet();
					var dbGenres = db.Genres.Select(g => g.ID).ToHashSet();
					foreach (int item in dbGenres)
					{
						if (selectedGenres.Contains(item) && !movieGenres.Contains(item))
						{
							db.MovGens.Add(new MovGen { MovieID = movie.ID, GenreID = item });
						}
						else if (movieGenres.Contains(item) && !selectedGenres.Contains(item))
						{
							db.MovGens.Remove(db.MovGens.Single(mg => mg.MovieID == movie.ID && mg.GenreID == item));
						}
					}
					await db.SaveChangesAsync();
					TempData["Status"] = "The changes you made to the movie have been saved!";
					TempData["Color"] = "success";
				}
				catch (DbUpdateConcurrencyException)
				{
					if (!MovieExists(movie.ID)) { return RedirectToAction("Page404", "Home"); }
					TempData["Status"] = "We're sorry, an unexpected error has been accured." + Environment.NewLine + "If you keep getting this error please contact system administrator.";
					TempData["Color"] = "danger";
					return RedirectToAction(nameof(Index));
				}
				return RedirectToAction(nameof(Info), new { id = movie.ID });
			}
			var model = new NewMovieViewModel(db);
			model.Movie = movie;
			model.Genres = model.EditGenres(GenreList);
			ViewData["PosterID"] = new SelectList(db.Images.Where(i => i.Category == ImageCategory.Posters), "ID", "Name", movie.PosterID);
			ViewData["RateID"] = new SelectList(db.Rates, "ID", "Code", movie.RateID);
			return View(model);
		}


		/* Delete movie */
		public async Task<IActionResult> Delete(int? id)
		{
			if (id == null) { return RedirectToAction("Page404", "Home"); }
			var dbMovie = await db.Movies
				.Include(m => m.Genres).ThenInclude(mg => mg.Genre)
				.Include(m => m.Poster)
				.Include(m => m.Rate)
				.SingleOrDefaultAsync(m => m.ID == id);
			if (dbMovie == null) { return RedirectToAction("Page404", "Home"); }
			return View(dbMovie);
		}

		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(int id)
		{
			var dbMovie = await db.Movies
				.Include(m => m.Genres)
				.SingleOrDefaultAsync(m => m.ID == id);
			if (dbMovie == null) { return RedirectToAction("Page404", "Home"); }
			try
			{
				//var movieGenres = db.MovGens.Where(mg => mg.MovieID == id).ToList();
				// Remove from movgens table first
				foreach (MovGen item in dbMovie.Genres)
				{
					db.MovGens.Remove(item);
				}
				db.Movies.Remove(dbMovie);
				await db.SaveChangesAsync();
				TempData["Status"] = "The movie has been successfully deleted!";
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
        private bool MovieExists(int id)
        {
            return db.Movies.Any(e => e.ID == id);
        }

		/* Server Validations */
		private void MovieDBExist(string mTitle, string mTrailer, int? id = null)
		{
			// Unique name
			var dbMovie = db.Movies.AsNoTracking().FirstOrDefault(m => m.Title == mTitle);
			if ((dbMovie != null && id == null) || (dbMovie != null && dbMovie.ID != id))
			{
				ModelState.AddModelError("Movie.Title", "A movie with that title already exists.");
			}
			// Unique trailer
			var dbMovieT = db.Movies.AsNoTracking().FirstOrDefault(m => m.Trailer == mTrailer);
			if ((dbMovieT != null && id == null) || (dbMovieT != null && dbMovieT.ID != id))
			{
				ModelState.AddModelError("Movie.Trailer", "A movie with that trailer already exists, another trailer maybe?");
			}
		}
	}
}
