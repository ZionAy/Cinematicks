using Cinematicks.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cinematicks.ViewModels
{
    public class OrderShowsViewModel
    {
		private readonly DBContext db;
		public DateTime ShowDate { get; set; }
		public string ShowMovie { get; set; }
		public Dictionary<Hall, List<ShowListItem>> ShowHalls { get; set; }
		public Dictionary<TimeSpan, List<ShowListItem>> ShowTimes { get; set; }

		public OrderShowsViewModel(DBContext context, int movieID, DateTime date)
		{
			db = context;
			ShowHalls = null;
			ShowTimes = null;
			//check for movie and date - correct or default for all movies and today.
			ShowDate = date < DateTime.Today ? DateTime.Today : date;
			var dbMovie = db.Movies.SingleOrDefault(m => m.ID == movieID);
			int chosenMovie = dbMovie != null ? dbMovie.ID : 0;
			ShowMovie = dbMovie != null ? dbMovie.Title : "All movies";
			//if movie chosen, populate list for it, else populate all movies on date.
			if (chosenMovie != 0) { ShowHalls = GetShowsByMovie(chosenMovie, ShowDate); }
			else { ShowTimes = GetShowsByDate(ShowDate); }
		}

		private Dictionary<Hall, List<ShowListItem>> GetShowsByMovie(int movieID, DateTime date)
		{
			var dicList = new Dictionary<Hall, List<ShowListItem>>();

			var timeShow = date == DateTime.Today ? DateTime.Now.TimeOfDay : new TimeSpan(0, 0, 0);

			var dbShows = db.Shows
					.Include(show => show.Hall)
					.Include(show => show.Movie)
					.Where(s => s.MovieID == movieID && s.ShowDate == date && s.ShowTime >= timeShow)
					.OrderBy(s => s.Hall).ThenBy(s => s.ShowDate).ThenBy(s => s.ShowTime)
					.ToList();

			foreach (var item in dbShows)
			{
				if (dicList.ContainsKey(item.Hall))
				{
					dicList.GetValueOrDefault(item.Hall).Add(new ShowListItem { ID = item.ID, Text = item.ShowTime.ToString(@"hh\:mm") });
				}
				else
				{
					dicList.Add(
							key: item.Hall,
							value: new List<ShowListItem>()
							{
														new ShowListItem
														{
																ID = item.ID,
																Text = item.ShowTime.ToString(@"hh\:mm")
														}
							});
				}
			}
			dicList.OrderBy(list => list.Key.ID);
			return dicList;
		}

		private Dictionary<TimeSpan, List<ShowListItem>> GetShowsByDate(DateTime date)
		{
			var dicList = new Dictionary<TimeSpan, List<ShowListItem>>();

			var timeShow = date == DateTime.Today ? DateTime.Now.TimeOfDay : new TimeSpan(0, 0, 0);

			var dbShows = db.Shows
					.Include(show => show.Hall)
					.Include(show => show.Movie)
					.Where(s => s.ShowDate == date && s.ShowTime >= timeShow)
					.OrderBy(s => s.ShowDate).ThenBy(s => s.ShowTime)
					.ToList();

			foreach (var item in dbShows)
			{
				if (dicList.ContainsKey(item.ShowTime))
				{
					dicList.GetValueOrDefault(item.ShowTime).Add(new ShowListItem { ID = item.ID, Text = item.Movie.Title });
				}
				else
				{
					dicList.Add(
							key: item.ShowTime,
							value: new List<ShowListItem>()
							{
														new ShowListItem
														{
																ID = item.ID,
																Text = item.Movie.Title
														}
							});
				}
			}

			dicList.OrderBy(list => list.Key);
			return dicList;
		}
	}
}
