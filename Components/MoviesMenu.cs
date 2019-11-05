using Cinematicks.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cinematicks.Components
{
    public class MoviesMenu : ViewComponent
    {
		private readonly DBContext db;
		public ICollection<Genre> GenreList { get; set; }

		public MoviesMenu(DBContext context)
		{
			db = context;
			GenreList = db.Genres
				.Where(g => g.InMenu == true)
				.ToList();
		}

		public IViewComponentResult Invoke()
		{
			return View(GenreList);
		}
	}
}
