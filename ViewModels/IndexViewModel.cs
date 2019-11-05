using Cinematicks.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cinematicks.ViewModels
{
    public class IndexViewModel
    {
		private readonly DBContext db;
		public List<Movie> ComingSoon { get; set; }
		public List<Movie> ShowingNow { get; set; }
		public List<Review> TopReviews { get; set; }

		public IndexViewModel(DBContext context)
		{
			db = context;
			Init();
		}

		private void Init()
		{
			ComingSoon = db.Movies
				.Include(m => m.Genres).ThenInclude(mg => mg.Genre)
				.Include(m => m.Poster)
				.Include(m => m.Rate).ThenInclude(rt => rt.Image)
				.Where(m => m.Release > DateTime.Today)
				.ToList();
			ShowingNow = db.Movies
				.Include(m => m.Genres).ThenInclude(mg => mg.Genre)
				.Include(m => m.Poster)
				.Include(m => m.Rate).ThenInclude(rt => rt.Image)
				.Include(m => m.Reviews)
				.Where(m => m.Shows.Count > 0 && m.Shows.Any(s => s.ShowDate > DateTime.Today || (s.ShowDate == DateTime.Today && s.ShowTime > DateTime.Now.TimeOfDay)))
				.ToList();
			TopReviews = db.Reviews
				.Include(rv => rv.Client).ThenInclude(cl => cl.Avatar)
				.Include(rv => rv.Movie).ThenInclude(m => m.Poster)
				.Where(rv => rv.Rating >= 8)
				.ToList();
		}
	}
}
