using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Cinematicks.Models;

namespace Cinematicks.Controllers
{
    public class SearchController : Controller
    {
        private readonly DBContext db;
        public SearchController(DBContext context)
        {
            db = context;
        }

        // GET: Search
        public async Task<IActionResult> S(string filter, string find)
		{
			if ((find != null && find != "" && find != " ") && (filter == "title" || filter == "director" || filter == "actor" || filter == "plot"))
			{
				var dbMovies = db.Movies
					.Include(m => m.Genres).ThenInclude(mg => mg.Genre)
					.Include(m => m.Rate).ThenInclude(rt => rt.Image)
					.Include(m => m.Poster)
					.Include(m => m.Reviews)
					.Where(m => m.Shows.Any(sh => sh.ShowDate > DateTime.Today || (sh.ShowDate == DateTime.Today && sh.ShowTime > DateTime.Now.TimeOfDay)));
				switch (filter)
				{
					case "title":
						ViewData["Filter"] = "title";
						dbMovies = dbMovies.Where(m => m.Title.ToUpper().Contains(find.ToUpper()));
						break;
					case "director":
						ViewData["Filter"] = "director";
						dbMovies = dbMovies.Where(m => m.Director.ToUpper().Contains(find.ToUpper()));
						break;
					case "actor":
						ViewData["Filter"] = "actor";
						dbMovies = dbMovies.Where(m => m.Actors.ToUpper().Contains(find.ToUpper()));
						break;
					case "plot":
						ViewData["Filter"] = "plot";
						dbMovies = dbMovies.Where(m => m.Plot.ToUpper().Contains(find.ToUpper()));
						break;
					default:
						return RedirectToAction("Index", "Movies");
				}
				ViewData["Find"] = find;
				return View(await dbMovies.ToListAsync());
			}
			else
			{
				return NotFound();
			}
        }
		
    }
}
