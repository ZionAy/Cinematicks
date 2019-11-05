using Cinematicks.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cinematicks.ViewModels
{
    public class DashViewModel
    {
		private readonly DBContext db;

		public Cinema Cinema { get; private set; }
		public Dictionary<string, int> Totals { get; private set; }
		public Dictionary<string, int> Movies { get; private set; }
		public Dictionary<string, int> Clients { get; private set; }
		public Dictionary<string, int> Promos { get; private set; }

		public DashViewModel(DBContext context)
		{
			db = context;
		}

		public async Task Init()
		{
			Cinema = await db.Cinemas
				.Include(ci => ci.Halls)
				.Include(ci => ci.Location).ThenInclude(l => l.Map)
				.Include(ci => ci.Photo)
				.SingleOrDefaultAsync(ci => ci.ID == 1);

			var dbClients = await db.Clients
				.Include(cl => cl.Orders)
				.Include(cl => cl.Reviews)
				.Select(cl => new { cl.RegisterTime, Orders = cl.Orders.Count(o => o.OrderTime > DateTime.Now.AddDays(-7)), Reviews = cl.Reviews.Count(rv => rv.PostTime > DateTime.Now.AddDays(-7)) })
				.ToListAsync();

			var dbMovies = await db.Movies
				.Include(m => m.Shows)
				.Select(m => new { m.Release, Shows = m.Shows.Count(s => s.ShowDate >= DateTime.Today) })
				.ToListAsync();

			var dbPromos = await db.Promos
				.Where(pr => pr.EndTime >= DateTime.Today)
				.Select(pr => new { Start = pr.StartTime, End = pr.EndTime })
				.ToListAsync();

			Totals = new Dictionary<string, int>
			{
				{ "Clients", dbClients.Count },
				{ "Movies", dbMovies.Count },
				{ "Genres", db.Genres.Count() },
				{ "Rates", db.Rates.Count() },
				{ "Gallery", db.Gallery.Count(ph => ph.IsActive == true) }
			};

			Movies = new Dictionary<string, int>
			{
				{ "Premiers", dbMovies.Count(m => m.Release >= DateTime.Today && m.Release <= DateTime.Today.AddDays(7)) },
				{ "Shows", dbMovies.Sum(m => m.Shows) }
			};

			Clients = new Dictionary<string, int>
			{
				{ "Register", dbClients.Count(cl => cl.RegisterTime > DateTime.Now.AddDays(-7)) },
				{ "Orders", dbClients.Sum(cl => cl.Orders) },
				{ "Reviews", dbClients.Sum(cl => cl.Reviews) }
			};

			Promos = new Dictionary<string, int>
			{
				{ "Running", dbPromos.Count(pr => pr.Start <= DateTime.Today) },
				{ "Starting", dbPromos.Count(pr => pr.Start >= DateTime.Today && pr.Start <= DateTime.Today.AddDays(7)) },
				{ "Ending", dbPromos.Count(pr => pr.End >= DateTime.Today && pr.End <= DateTime.Today.AddDays(7)) }
			};
		}
	}
}
