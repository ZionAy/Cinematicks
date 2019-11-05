using Cinematicks.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cinematicks.ViewModels
{
    public class NewOrderViewModel
    {
		private readonly DBContext db;
		public List<MovieListItem> MoviesList { get; set; }
		public List<DateListItem> DatesList { get; set; }
		public OrderShowsViewModel ShowsTable { get; set; }

		public NewOrderViewModel(DBContext context, int? movieID = null)
		{
			db = context;
			MoviesList = db.Movies
				.Where(m => m.Shows.Any(s => s.ShowDate > DateTime.Today || (s.ShowDate == DateTime.Today && s.ShowTime > DateTime.Now.TimeOfDay)))
				.Select(m => new MovieListItem { ID = m.ID, Title = m.Title })
				.OrderBy(ml => ml.Title)
				.ToList();

			DatesList = db.Shows
				.Where(s => s.ShowDate >= DateTime.Today)
				.Select(s => s.ShowDate)
				.Distinct()
				.OrderBy(s => s.Date)
				.Select(s => new DateListItem { Text = s.Date.ToString("dd/MM/yyyy"), Value = s.Date.ToString("yyyy-MM-dd") })
				.ToList();

			ShowsTable = new OrderShowsViewModel(db, movieID ?? 0, DateTime.Today);
		}
	}

	public class MovieListItem
	{
		public int ID { get; set; }
		public string Title { get; set; }
	}
	public class DateListItem
	{
		public string Text { get; set; }
		public string Value { get; set; }
	}
	public class ShowListItem
	{
		public int ID { get; set; }
		public string Text { get; set; }
	}
}
