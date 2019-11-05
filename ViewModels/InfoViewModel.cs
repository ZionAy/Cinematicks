using Cinematicks.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cinematicks.ViewModels
{
    public class InfoViewModel
    {
		private readonly DBContext db;
		public Cinema Cinema { get; set; }
		public List<PhotoGal> Gallery { get; set; }

		public InfoViewModel(DBContext context)
		{
			db = context;
			Init();
		}

		private void Init()
		{
			Cinema = db.Cinemas
				.Include(ci => ci.Location).ThenInclude(l => l.Map)
				.Include(ci => ci.Photo)
				.SingleOrDefault(ci => ci.ID == 1);

			Gallery = db.Gallery
				.Include(ph => ph.Photo)
				.Where(ph => ph.CinemaID == 1 && ph.IsActive == true)
				.ToList();
		}
	}
}
