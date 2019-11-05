using Cinematicks.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cinematicks.ViewModels
{
    public class NewMovieViewModel
    {
		private readonly DBContext db;
		public Movie Movie { get; set; }
		public List<GenresInMovie> Genres { get; set; }

		public NewMovieViewModel(DBContext context, int? movieID = null)
		{
			db = context;
			var id = movieID ?? 0;
			Movie = db.Movies.SingleOrDefault(m => m.ID == id);
			Genres = PopulateGenres(id);
		}

		private List<GenresInMovie> PopulateGenres(int id)
		{
			var list = new List<GenresInMovie>();
			var dbGenres = db.Genres.ToList();
			var movieGenres = db.MovGens.Where(mg => mg.MovieID == id).Select(mg => mg.GenreID).ToHashSet();
			foreach (Genre genre in dbGenres)
			{
				list.Add(new GenresInMovie()
				{
					GenreID = genre.ID,
					GenreName = genre.Name,
					InMovie = movieGenres.Contains(genre.ID)
				});
			}
			return list;
		}

		public List<GenresInMovie> EditGenres(int[] genres)
		{
			var list = new List<GenresInMovie>();
			var dbGenres = db.Genres.ToList();
			var movieGenres = genres.ToHashSet();
			foreach (Genre genre in dbGenres)
			{
				list.Add(new GenresInMovie()
				{
					GenreID = genre.ID,
					GenreName = genre.Name,
					InMovie = movieGenres.Contains(genre.ID)
				});
			}
			return list;
		}
	}

	public class GenresInMovie
	{
		public int GenreID { get; set; }
		public string GenreName { get; set; }
		public bool InMovie { get; set; }
	}
}
