using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cinematicks.Models
{
    public class DBContext : IdentityDbContext<Client>
	{
		public DBContext(DbContextOptions<DBContext> options) : base(options) { }

		public DbSet<Image> Images { get; set; }
		public DbSet<Client> Clients { get; set; }
		public DbSet<Cinema> Cinemas { get; set; }
		public DbSet<Genre> Genres { get; set; }
		public DbSet<Hall> Halls { get; set; }
		public DbSet<Location> Locations { get; set; }
		public DbSet<MovGen> MovGens { get; set; }
		public DbSet<Movie> Movies { get; set; }
		public DbSet<Order> Orders { get; set; }
		public DbSet<Payment> Payments { get; set; }
		public DbSet<PhotoGal> Gallery { get; set; }
		public DbSet<Promo> Promos { get; set; }
		public DbSet<Rate> Rates { get; set; }
		public DbSet<Review> Reviews { get; set; }
		public DbSet<Show> Shows { get; set; }
		public DbSet<Ticket> Tickets { get; set; }

		protected override void OnModelCreating(ModelBuilder builder)
		{
			base.OnModelCreating(builder);

			/* DB tables relationships */
			//Image - Clients (Avatars)
			builder.Entity<Client>()
				.HasOne(cl => cl.Avatar)
				.WithMany(i => i.Avatars)
				.HasForeignKey(cl => cl.AvatarID)
				.OnDelete(DeleteBehavior.Restrict);
			//Image - Cinemas:
			builder.Entity<Cinema>()
				.HasOne(ci => ci.Photo)
				.WithMany(i => i.Cinemas)
				.HasForeignKey(ci => ci.PhotoID)
				.OnDelete(DeleteBehavior.Restrict);
			//Image - Rates:
			builder.Entity<Rate>()
				.HasOne(rt => rt.Image)
				.WithMany(i => i.Rates)
				.HasForeignKey(rt => rt.ImageID)
				.OnDelete(DeleteBehavior.Restrict);
			//Image - Locations:
			builder.Entity<Location>()
				.HasOne(l => l.Map)
				.WithMany(i => i.Locations)
				.HasForeignKey(l => l.MapID)
				.OnDelete(DeleteBehavior.Restrict);
			//Image - PhotoGals:
			builder.Entity<PhotoGal>()
				.HasOne(ph => ph.Photo)
				.WithMany(i => i.Gallery)
				.HasForeignKey(ph => ph.PhotoID)
				.OnDelete(DeleteBehavior.Restrict);
			//Image - Movies:
			builder.Entity<Movie>()
				.HasOne(m => m.Poster)
				.WithMany(i => i.Movies)
				.HasForeignKey(m => m.PosterID)
				.OnDelete(DeleteBehavior.Restrict);
			//Image - Promos:
			builder.Entity<Promo>()
				.HasOne(pr => pr.Banner)
				.WithMany(i => i.Banners)
				.HasForeignKey(pr => pr.BannerID)
				.OnDelete(DeleteBehavior.Restrict);
			//Location - Cinemas:
			builder.Entity<Cinema>()
				.HasOne(ci => ci.Location)
				.WithMany(l => l.Cinemas)
				.HasForeignKey(ci => ci.LocationID)
				.OnDelete(DeleteBehavior.Restrict);
			//Cinema - Halls:
			builder.Entity<Hall>()
				.HasOne(h => h.Cinema)
				.WithMany(ci => ci.Halls)
				.HasForeignKey(h => h.CinemaID)
				.OnDelete(DeleteBehavior.Restrict);
			//Cinema - PhotoGals:
			builder.Entity<PhotoGal>()
				.HasOne(ph => ph.Cinema)
				.WithMany(ci => ci.Gallery)
				.HasForeignKey(ph => ph.CinemaID)
				.OnDelete(DeleteBehavior.Restrict);
			//Rate - Movies:
			builder.Entity<Movie>()
				.HasOne(m => m.Rate)
				.WithMany(rt => rt.Movies)
				.HasForeignKey(m => m.RateID)
				.OnDelete(DeleteBehavior.Restrict);
			//Client - Reviews:
			builder.Entity<Review>()
				.HasOne(rv => rv.Client)
				.WithMany(cl => cl.Reviews)
				.HasForeignKey(rv => rv.ClientID)
				.OnDelete(DeleteBehavior.Restrict);
			//Movie - Reviews:
			builder.Entity<Review>()
				.HasOne(rv => rv.Movie)
				.WithMany(m => m.Reviews)
				.HasForeignKey(rv => rv.MovieID)
				.OnDelete(DeleteBehavior.Restrict);
			//Movie - Shows:
			builder.Entity<Show>()
				.HasOne(s => s.Movie)
				.WithMany(m => m.Shows)
				.HasForeignKey(s => s.MovieID)
				.OnDelete(DeleteBehavior.Restrict);
			//Hall - Shows:
			builder.Entity<Show>()
				.HasOne(s => s.Hall)
				.WithMany(h => h.Shows)
				.HasForeignKey(s => s.HallID)
				.OnDelete(DeleteBehavior.Restrict);
			//Show - Tickets:
			builder.Entity<Ticket>()
				.HasOne(t => t.Show)
				.WithMany(s => s.Tickets)
				.HasForeignKey(t => t.ShowID)
				.OnDelete(DeleteBehavior.Restrict);
			//Client - Tickets:
			builder.Entity<Ticket>()
				.HasOne(t => t.Client)
				.WithMany(cl => cl.Tickets)
				.HasForeignKey(t => t.ClientID)
				.OnDelete(DeleteBehavior.Restrict);
			//Client - Orders:
			builder.Entity<Order>()
				.HasOne(o => o.Client)
				.WithMany(cl => cl.Orders)
				.HasForeignKey(o => o.ClientID)
				.OnDelete(DeleteBehavior.Restrict);
			//Order - Tickets:
			builder.Entity<Ticket>()
				.HasOne(t => t.Order)
				.WithMany(o => o.Tickets)
				.HasForeignKey(t => t.OrderID)
				.OnDelete(DeleteBehavior.Cascade);

			//Movies - Genres (many to many with MovGens as join table):
			builder.Entity<MovGen>()
				.HasKey(mg => new { mg.MovieID, mg.GenreID });
			builder.Entity<MovGen>()
				.HasOne(mg => mg.Movie)
				.WithMany(m => m.Genres)
				.HasForeignKey(mg => mg.MovieID)
				.OnDelete(DeleteBehavior.Restrict);
			builder.Entity<MovGen>()
				.HasOne(mg => mg.Genre)
				.WithMany(g => g.Movies)
				.HasForeignKey(mg => mg.GenreID)
				.OnDelete(DeleteBehavior.Restrict);

			//Order-Payment: one to one
			builder.Entity<Order>()
				.HasOne(o => o.Payment)
				.WithOne(py => py.Order)
				.HasForeignKey<Payment>(p => p.OrderID)
				.OnDelete(DeleteBehavior.Cascade);


			/* DB unique fields in tables columns */
			builder.Entity<Cinema>().HasIndex(ci => ci.Name).IsUnique();
			builder.Entity<Genre>().HasIndex(g => g.Name).IsUnique();
			builder.Entity<Hall>().HasIndex(h => new { h.CinemaID, h.Name }).IsUnique();
			builder.Entity<Image>().HasIndex(i => new { i.Category, i.Name }).IsUnique();
			builder.Entity<Image>().HasIndex(i => i.Filename).IsUnique();
			builder.Entity<Location>().HasIndex(l => new { l.City, l.Address }).IsUnique();
			builder.Entity<Movie>().HasIndex(m => m.Title).IsUnique();
			builder.Entity<Movie>().HasIndex(m => m.Trailer).IsUnique();
			builder.Entity<PhotoGal>().HasIndex(p => new { p.CinemaID, p.Title }).IsUnique();
			builder.Entity<Promo>().HasIndex(d => d.Name).IsUnique();
			builder.Entity<Rate>().HasIndex(r => r.Code).IsUnique();
			builder.Entity<Review>().HasIndex(rv => new { rv.ClientID, rv.MovieID }).IsUnique();
			builder.Entity<Show>().HasIndex(s => new { s.HallID, s.ShowDate, s.ShowTime }).IsUnique();
			builder.Entity<Ticket>().HasIndex(t => new { t.ShowID, t.Row, t.Col }).IsUnique();
			/* If you are not using Identity system, use this lines */
			//builder.Entity<Client>().HasIndex(cl => cl.UserName).IsUnique();
			//builder.Entity<Client>().HasIndex(cl => cl.NormalizedUserName).IsUnique();
			//builder.Entity<Client>().HasIndex(cl => cl.Email).IsUnique();
			//builder.Entity<Client>().HasIndex(cl => cl.NormalizedEmail).IsUnique();
		}

	}
}
