using Cinematicks.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cinematicks.Data
{
    public class DemoData
    {
		private readonly DBContext db;
		private readonly UserManager<Client> userManager;
		private readonly RoleManager<IdentityRole> roleManager;

		public DemoData(DBContext context, UserManager<Client> uManager, RoleManager<IdentityRole> rManager)
		{
			db = context;
			userManager = uManager;
			roleManager = rManager;

			/* Check if the db is already created or create it. */
			db.Database.Migrate();

			StartSeed();
		}

		private void StartSeed()
		{
			/* Check if there is data in the db for every section. If there is no data, seed it. */
			if (!db.Images.Any()) { ImageSeed(); }
			if (!db.Roles.Any()) { RoleSeed(); }
			if (!db.Clients.Any()) { ClientSeed(); }
			if (!db.UserRoles.Any()) { RoleAssignSeed(); }
			if (!db.Locations.Any()) { LocationSeed(); }
			if (!db.Cinemas.Any()) { CinemaSeed(); }
			if (!db.Halls.Any()) { HallSeed(); }
			if (!db.Gallery.Any()) { GallerySeed(); }
			if (!db.Promos.Any()) { PromoSeed(); }
			if (!db.Rates.Any()) { RateSeed(); }
			if (!db.Genres.Any()) { GenreSeed(); }
			if (!db.Movies.Any()) { MovieSeed(); }
			if (!db.Shows.Any()) { ShowSeed(); }
			if (!db.Reviews.Any()) { ReviewSeed(); }
			if (!db.Orders.Any()) { OrderSeed(); }
		}

		private void ImageSeed()
		{
			// Notes:
			//   * Filename property (Guid) is hardcoded here based on the images already on server.
			var demoImages = new Image[]
			{
				// Avatars
				new Image {
					Name = "No Avatar", Category = ImageCategory.Avatars,
					Filename = "_NoAvatar.png"
				},
				new Image {
					Name = "Panda", Category = ImageCategory.Avatars,
					Filename = "cad87922-2258-4f2d-8d3f-a935528f00a9.png"
				},
				new Image {
					Name = "Hippo", Category = ImageCategory.Avatars,
					Filename = "452a091c-b91c-44c9-9e79-a60534a8592c.png"
				},
				new Image {
					Name = "Monkey", Category = ImageCategory.Avatars,
					Filename = "0f92035f-ea42-409d-b7bb-08cf96c8025f.png"
				},
				new Image {
					Name = "Tiger", Category = ImageCategory.Avatars,
					Filename = "22cf29fe-aa45-485d-9854-e3d9f4bf3ce7.png"
				},
				new Image {
					Name = "Rabbit", Category = ImageCategory.Avatars,
					Filename = "239f9331-f8b8-4c4c-b4cc-d64668ccc4cf.png"
				},
				// Banners
				new Image {
					Name = "Banner 1", Category = ImageCategory.Banners,
					Filename = "30fab2dc-9a6b-44ec-908e-095b64c858f4.jpg"
				},
				new Image {
					Name = "Banner 2", Category = ImageCategory.Banners,
					Filename = "3c7bc599-a5f9-4d30-8e4d-ee9eb59f3708.jpg"
				},
				new Image {
					Name = "Banner 3", Category = ImageCategory.Banners,
					Filename = "8d7e1908-deba-4f3c-b08d-c2c00b7f43a6.jpg"
				},
				new Image {
					Name = "Banner 4", Category = ImageCategory.Banners,
					Filename = "1c57d8ed-b3b4-4c26-99e8-428712988814.jpg"
				},
				new Image {
					Name = "Banner 5", Category = ImageCategory.Banners,
					Filename = "3a239c4d-b59f-434c-be4b-4a03c9e91ad9.jpg"
				},
				// Gallery
				new Image {
					Name = "Cinema", Category = ImageCategory.Gallery,
					Filename = "a47aa28d-05bf-4034-b779-861d0e7c4c09.jpg"
				},
				new Image {
					Name = "Hall 1", Category = ImageCategory.Gallery,
					Filename = "2b7ca93d-5b46-4585-9876-5ed38925b711.jpg"
				},
				new Image {
					Name = "Hall 2", Category = ImageCategory.Gallery,
					Filename = "e96bae1a-c006-4ad8-9995-a5bbf2ce7531.jpg"
				},
				new Image {
					Name = "Hall 3", Category = ImageCategory.Gallery,
					Filename = "f8c7bd1d-a5fd-4af7-a18f-4a2b34e4b4ed.jpg"
				},
				new Image {
					Name = "Cashiers", Category = ImageCategory.Gallery,
					Filename = "12f30873-af06-4bb0-a803-c566073bac37.jpg"
				},
				new Image {
					Name = "Patio", Category = ImageCategory.Gallery,
					Filename = "39b1a746-6537-4939-8775-df65be19e9c6.jpg"
				},
				new Image {
					Name = "Snack Bar", Category = ImageCategory.Gallery,
					Filename = "b6779d63-90ab-4d7d-a2ab-f9d2a9145179.jpg"
				},
				new Image {
					Name = "Clients 1", Category = ImageCategory.Gallery,
					Filename = "f18c43c3-06bf-4b4e-836f-afd1e1c24f61.jpg"
				},
				new Image {
					Name = "Clients 2", Category = ImageCategory.Gallery,
					Filename = "a47116f1-0d8e-4e28-b858-8dce63bd9551.jpg"
				},
				new Image {
					Name = "Clients 3", Category = ImageCategory.Gallery,
					Filename = "14c7cf1b-0141-4284-986f-3b6a91ddee66.jpg"
				},
				// Maps
				new Image {
					Name = "Map", Category = ImageCategory.Maps,
					Filename = "812f82ea-250a-469b-9e9f-15284d717299.jpg"
				},
				// Posters
				new Image {
					Name = "No Poster", Category = ImageCategory.Posters,
					Filename = "_NoPoster.jpg"
				},
				new Image {
					Name = "Cars", Category = ImageCategory.Posters,
					Filename = "eca6dbf8-7073-4d81-a9cb-b29194fc8569.jpg"
				},
				new Image {
					Name = "Killers", Category = ImageCategory.Posters,
					Filename = "82cfa890-b186-46bd-ba48-bc6c4089e85e.jpg"
				},
				new Image {
					Name = "So Undercover", Category = ImageCategory.Posters,
					Filename = "e2c4ee9d-64cb-49d1-ac5e-686029041306.jpg"
				},
				new Image {
					Name = "Walk Of Shame", Category = ImageCategory.Posters,
					Filename = "cd2324f4-dba1-4800-adf1-59f0e8e38ae2.jpg"
				},
				new Image {
					Name = "Vehicle 19", Category = ImageCategory.Posters,
					Filename = "d715b766-dc8f-4e53-8b3f-61dff9936b6e.jpg"
				},
				new Image {
					Name = "Time Lapse", Category = ImageCategory.Posters,
					Filename = "634aaeb2-4f21-42f0-9bc9-7b8dff33c71f.jpg"
				},
				new Image {
					Name = "The Nut Job", Category = ImageCategory.Posters,
					Filename = "347730f6-f939-4d28-bcbf-7d1897ebbf8f.jpg"
				},
				new Image {
					Name = "The Purge Anarchy", Category = ImageCategory.Posters,
					Filename = "6b99ce6d-5253-4362-bd4b-4280ab7f3230.jpg"
				},
				new Image {
					Name = "The Machine", Category = ImageCategory.Posters,
					Filename = "b482cafa-eddb-4e54-906d-f067b23caccc.jpg"
				},
				new Image {
					Name = "White House Down", Category = ImageCategory.Posters,
					Filename = "7a0552d1-f876-4e09-a5ef-3467d2dbd433.jpg"
				},
				new Image {
					Name = "Were The Millers", Category = ImageCategory.Posters,
					Filename = "5ca39363-6980-4711-b8f4-e2c0f785f27e.jpg"
				},
				new Image {
					Name = "Woman In Gold", Category = ImageCategory.Posters,
					Filename = "efcf5bee-ab04-4dca-9e1d-89e08ecbc3ba.jpg"
				},
				new Image {
					Name = "Last Vegas", Category = ImageCategory.Posters,
					Filename = "c1b5b40a-a3a7-46c7-ad20-e0b593c4f618.jpg"
				},
				new Image {
					Name = "Noah", Category = ImageCategory.Posters,
					Filename = "48749427-3864-4e60-a6dd-d1b885dec6e6.jpg"
				},
				new Image {
					Name = "The Hobbit The Desolation Of Smaug", Category = ImageCategory.Posters,
					Filename = "a41d7055-f402-41fd-904e-f7a9972ce311.jpg"
				},
				new Image {
					Name = "Rio", Category = ImageCategory.Posters,
					Filename = "2e89cfa6-aec3-47fc-b220-39c1c626ae8e.jpg"
				},
				new Image {
					Name = "Tracers", Category = ImageCategory.Posters,
					Filename = "3f22edca-6a7b-49d2-8215-4648cada0347.jpg"
				},
				new Image {
					Name = "Frozen", Category = ImageCategory.Posters,
					Filename = "a0fa5236-ba9b-40e6-a69e-69064e99764d.jpg"
				},				
				// Rates
				new Image {
					Name = "Rate G", Category = ImageCategory.Rates,
					Filename = "597c78e9-3dc5-47a5-86aa-ca481cfb014d.png"
				},
				new Image {
					Name = "Rate NC-17", Category = ImageCategory.Rates,
					Filename = "2d174411-325e-46bb-baa1-f282ebd89c17.png"
				},
				new Image {
					Name = "Rate PG", Category = ImageCategory.Rates,
					Filename = "0f4965d6-a854-43b8-9e0c-77ad4c75fe0f.png"
				},
				new Image {
					Name = "Rate PG-13", Category = ImageCategory.Rates,
					Filename = "b76ad0f8-660c-42b5-beee-3a39831a861a.png"
				},
				new Image {
					Name = "Rate R", Category = ImageCategory.Rates,
					Filename = "e5547aed-bc4b-469c-bef4-ab6b7540c327.png"
				}
			};
			foreach (Image img in demoImages)
			{
				db.Images.Add(img);
			}
			db.SaveChanges();
		}

		private void RoleSeed()
		{
			var demoRoles = new string[]
			{
				"Admin", "Manager", "Employee", "VIP"
			};
			foreach (string item in demoRoles)
			{
				var result = roleManager.CreateAsync(new IdentityRole(item)).Result;
			}
		}

		private void ClientSeed()
		{
			var demoClients = new Dictionary<Client, string>()
			{
				{
					new Client
					{
						UserName = "Admin", Email = "cinematicks1@gmail.com", EmailConfirmed = true,
						RegisterTime = DateTime.Now.AddDays(-7),
						AvatarID = db.Images.Single(i => i.Category == ImageCategory.Avatars && i.Name == "Monkey").ID
					}, "aa123456"
				},
				{
					new Client
					{
						UserName = "ZionAy", Email = "zion@scippy.co.il", EmailConfirmed = true,
						RegisterTime = DateTime.Now.AddDays(-7),
						AvatarID = db.Images.Single(i => i.Category == ImageCategory.Avatars && i.Name == "Monkey").ID
					}, "zz123456"
				},
				{
					new Client
					{
						UserName = "Scippy", Email = "scippy@scippy.co.il", EmailConfirmed = true,
						RegisterTime = DateTime.Now.AddDays(-7),
						AvatarID = db.Images.Single(i => i.Category == ImageCategory.Avatars && i.Name == "No Avatar").ID
					}, "ss123456"
				},
				{
					new Client
					{
						UserName = "MrMovie", Email = "mrmovie@scippy.co.il", EmailConfirmed = true,
						RegisterTime = DateTime.Now.AddDays(-7),
						AvatarID = db.Images.Single(i => i.Category == ImageCategory.Avatars && i.Name == "No Avatar").ID
					}, "mm123456"
				},
				{
					new Client
					{
						UserName = "HappyHippo", Email = "hippo@scippy.co.il", EmailConfirmed = true,
						RegisterTime = DateTime.Now.AddDays(-7),
						AvatarID = db.Images.Single(i => i.Category == ImageCategory.Avatars && i.Name == "Hippo").ID
					}, "hh123456"
				},
				{
					new Client
					{
						UserName = "PandaBear", Email = "panda@scippy.co.il", EmailConfirmed = true,
						RegisterTime = DateTime.Now.AddDays(-7),
						AvatarID = db.Images.Single(i => i.Category == ImageCategory.Avatars && i.Name == "Panda").ID
					}, "pp123456"
				}
			};
			foreach (KeyValuePair<Client, string> item in demoClients)
			{
				var client = item.Key;
				var pass = item.Value;
				var result = userManager.CreateAsync(client, pass).Result;
			}
		}

		private void RoleAssignSeed()
		{
			var demoAssigns = new Dictionary<string, string>
			{
				{ "Admin", "Admin" },
				{ "ZionAy", "Admin" },
				{ "Scippy", "Manager" }
			};
			foreach (KeyValuePair<string, string> item in demoAssigns)
			{
				var client = userManager.FindByNameAsync(item.Key).Result;
				if (client != null)
				{
					var result = userManager.AddToRoleAsync(client, item.Value).Result;
				}
			}
		}

		private void LocationSeed()
		{
			var demoLocations = new Location[]
			{
				new Location
				{
					City = "Tel Aviv", Address = "Derech HaTayasim 28",
					Directions = "The bus stops near us:" + Environment.NewLine + "line 12 from TLV." + Environment.NewLine + "line 20 from Haifa." + Environment.NewLine + "line 30from Eilat.",
					MapID = db.Images.Single(i => i.Category == ImageCategory.Maps && i.Name == "Map").ID
				}
			};
			foreach (Location location in demoLocations)
			{
				db.Locations.Add(location);
			}
			db.SaveChanges();
		}

		private void CinemaSeed()
		{
			var demoCinemas = new Cinema[]
			{
				new Cinema
				{
					Name = "Cinematicks", Price = 50,
					PhotoID = db.Images.Single(i => i.Category == ImageCategory.Gallery && i.Name == "Cinema").ID,
					LocationID = db.Locations.Single(l => l.City == "Tel Aviv" && l.Address == "Derech HaTayasim 28").ID,
					About = "Cinematicks is a web application created for a school project by Zion Ayooki." + Environment.NewLine + "Cinematicks main idea is to provide cinemas and movie theatres an easy to use and affordable web application to manage their cinema or movie theatre online." + Environment.NewLine + "Cinematicks project main idea was to learn more about web applications technology. On this project we used both server side and clieent side technologies to create a beutifull, intuitive and easy to use user interface to control all the aspects of the online ticket ordering proccess." + Environment.NewLine + "Some of the technologies, frameworks and libraries used on this project includes ASP.NET Core 2.0, MVC 6 Architecture for software development, C# and Razor, Entity Framework Core, Bootstrap, Jquery and more." + Environment.NewLine + "I hope you will enjoy using this web application as much as I did enjoy creating it."
				}
			};
			foreach (Cinema cinema in demoCinemas)
			{
				db.Cinemas.Add(cinema);
			}
			db.SaveChanges();
		}

		private void HallSeed()
		{
			var demoHalls = new Hall[]
			{
				new Hall
				{
					Name = "Hall 1", Rows = 10, Cols = 15,
					CinemaID = db.Cinemas.Single(ci => ci.Name == "Cinematicks").ID
				},
				new Hall
				{
					Name = "Hall 2", Rows = 10, Cols = 15,
					CinemaID = db.Cinemas.Single(ci => ci.Name == "Cinematicks").ID
				},
				new Hall
				{
					Name = "Hall 3", Rows = 10, Cols = 15,
					CinemaID = db.Cinemas.Single(ci => ci.Name == "Cinematicks").ID
				},
				new Hall
				{
					Name = "Hall 4", Rows = 10, Cols = 15,
					CinemaID = db.Cinemas.Single(ci => ci.Name == "Cinematicks").ID
				}
			};
			foreach (Hall hall in demoHalls)
			{
				db.Halls.Add(hall);
			}
			db.SaveChanges();
		}

		private void GallerySeed()
		{
			// Notes:
			// * IsActive is default for false, only true is declared
			var demoGallery = new PhotoGal[]
			{
				new PhotoGal
				{
					Title = "Lighting the night", Description = "Our cinema can be spoted from anywhere.", IsActive = true,
					CinemaID = db.Cinemas.Single(ci => ci.Name == "Cinematicks").ID,
					PhotoID = db.Images.Single(i => i.Category == ImageCategory.Gallery && i.Name == "Cinema").ID
				},
				new PhotoGal
				{
					Title = "Fun day", Description = "Our monthly fun day for our staff.", IsActive = true,
					CinemaID = db.Cinemas.Single(ci => ci.Name == "Cinematicks").ID,
					PhotoID = db.Images.Single(i => i.Category == ImageCategory.Gallery && i.Name == "Clients 1").ID
				},
				new PhotoGal
				{
					Title = "Yummy Yummy", Description = "Our snack bar is so tasty.",
					CinemaID = db.Cinemas.Single(ci => ci.Name == "Cinematicks").ID,
					PhotoID = db.Images.Single(i => i.Category == ImageCategory.Gallery && i.Name == "Snack Bar").ID
				},
				new PhotoGal
				{
					Title = "Halls", Description = "This is one of our halls you can seat.", IsActive = true,
					CinemaID = db.Cinemas.Single(ci => ci.Name == "Cinematicks").ID,
					PhotoID = db.Images.Single(i => i.Category == ImageCategory.Gallery && i.Name == "Hall 1").ID
				},
				new PhotoGal
				{
					Title = "VIP", Description = "Our VIP clients got free tickets to this show.", IsActive = true,
					CinemaID = db.Cinemas.Single(ci => ci.Name == "Cinematicks").ID,
					PhotoID = db.Images.Single(i => i.Category == ImageCategory.Gallery && i.Name == "Clients 3").ID
				}
			};
			foreach (PhotoGal photo in demoGallery)
			{
				db.Gallery.Add(photo);
			}
			db.SaveChanges();
		}

		private void PromoSeed()
		{
			var demoPromos = new Promo[]
			{
				new Promo
				{
					Name = "Students 2 plus 1",
					StartTime = DateTime.Today.AddDays(-10), EndTime = DateTime.Today.AddDays(30),
					Description = "This is for the students on Cinema College. For every 2 tickets that you buy, you will get 1 ticket for free. Please be noted: You must enter coupon code on the order checkout. Also be noted: You need to choose at leaset 3 tickets for the discount to take place on the 3rd ticket.",
					BannerID = db.Images.Single(i => i.Category == ImageCategory.Banners && i.Name == "Banner 1").ID
				},
				new Promo
				{
					Name = "3 Years Anniversary",
					StartTime = DateTime.Today, EndTime = DateTime.Today.AddDays(7),
					Description = "Only for one week. Our cinema is celebrating its 3rd birthday. Come and celebrate with us with a 10% discount on every ticket. Please be noted: You must enter coupon code on the order checkout.",
					BannerID = db.Images.Single(i => i.Category == ImageCategory.Banners && i.Name == "Banner 2").ID
				},
				new Promo
				{
					Name = "Family Night Out",
					StartTime = DateTime.Today.AddDays(-30), EndTime = DateTime.Today.AddDays(60),
					Description = "For this season we are coming out with a deal for every family. For every 3 tickets you buy you will get 1 ticket for free, come with your family or with your friends that we believe are like family, and have fun. Please be noted: You must enter coupon code on the order checkout. Also be noted: You need to choose at leaset 4 tickets for the discount to take place on the 4th ticket.",
					BannerID = db.Images.Single(i => i.Category == ImageCategory.Banners && i.Name == "Banner 3").ID
				},
				new Promo
				{
					Name = "Good Night",
					StartTime = DateTime.Today.AddDays(7), EndTime = DateTime.Today.AddDays(14),
					Description = "For one week only, we give you a night full of stars. 1+1 on all the tickets. For every ticket that you buy, you will get 1 ticket for free. Please be noted: You must enter coupon code on the order checkout. Also be noted: You need to choose at leaset 2 tickets for the discount to take place on the 2nd ticket.",
					BannerID = db.Images.Single(i => i.Category == ImageCategory.Banners && i.Name == "Banner 4").ID
				}
			};
			foreach (Promo promo in demoPromos)
			{
				db.Promos.Add(promo);
			}
			db.SaveChanges();
		}

		private void RateSeed()
		{
			var demoRates = new Rate[]
			{
				new Rate {
					Code = "G", Description = "All ages admitted", MinAge = 0,
					ImageID = db.Images.Single(i => i.Category == ImageCategory.Rates && i.Name == "Rate G").ID
				},
				new Rate {
					Code = "PG", Description = "Some material may not be suitable for children", MinAge = 12,
					ImageID = db.Images.Single(i => i.Category == ImageCategory.Rates && i.Name == "Rate PG").ID
				},
				new Rate {
					Code = "PG-13", Description = "Some material may be inappropriate for children under 13", MinAge = 13,
					ImageID = db.Images.Single(i => i.Category == ImageCategory.Rates && i.Name == "Rate PG-13").ID
				},
				new Rate {
					Code = "R", Description = "Under 17 required accompanying parent or adult guardian", MinAge = 16,
					ImageID = db.Images.Single(i => i.Category == ImageCategory.Rates && i.Name == "Rate R").ID
				},
				new Rate {
					Code = "NC-17", Description = "No one 17 and under admitted", MinAge = 18,
					ImageID = db.Images.Single(i => i.Category == ImageCategory.Rates && i.Name == "Rate NC-17").ID
				}
			};
			foreach (Rate rate in demoRates)
			{
				db.Rates.Add(rate);
			}
			db.SaveChanges();
		}

		private void GenreSeed()
		{
			// Notes:
			// * InMenu is default for false, only true is declared
			var demoGenres = new Genre[]
			{
				new Genre { Name = "Action", Description = "Action movies", InMenu = true },
				new Genre { Name = "Comedy", Description = "Comedy movies", InMenu = true },
				new Genre { Name = "Drama", Description = "Drama movies", InMenu = true },
				new Genre { Name = "Animation", Description = "Animation movies", InMenu = true },
				new Genre { Name = "Horror", Description = "Horror movies" },
				new Genre { Name = "Sci-Fi", Description = "Science fiction movies" },
				new Genre { Name = "Thriller", Description = "Thriller movies" }
			};
			foreach (Genre genre in demoGenres)
			{
				db.Genres.Add(genre);
			}
			db.SaveChanges();
		}

		private void MovieSeed()
		{
			var demoMovies = new Movie[]
			{
				new Movie
				{
					Title = "Cars Dubbed", Release = DateTime.Parse("2006-06-22"), Time = 117,
					Director = "John Lasseter", Actors = "Owen Wilson, Bonnie Hunt, Paul Newman",
					Plot = "A hot-shot race-car named Lightning McQueen gets waylaid in Radiator Springs, where he finds the true meaning of friendship and family.",
					Trailer = "WGByijP0Leo", IsDub = true,
					PosterID = db.Images.Single(i => i.Category == ImageCategory.Posters && i.Name == "Cars").ID,
					RateID = db.Rates.Single(r => r.Code == "G").ID,
					Genres = new List<MovGen>()
					{
						new MovGen { GenreID = db.Genres.Single(g => g.Name == "Animation").ID },
						new MovGen { GenreID = db.Genres.Single(g => g.Name == "Comedy").ID }
					}
				},
				new Movie
				{
					Title = "Killers", Release = DateTime.Parse("2010-06-03"), Time = 100,
					Director = "Robert Luketic", Actors = "Katherine Heigl, Ashton Kutcher, Tom Selleck",
					Plot = "A vacationing woman meets her ideal man, leading to a swift marriage. Back at home, however, their idyllic life is upset when they discover their neighbors could be assassins who have been contracted to kill the couple.",
					Trailer = "zV8o48-GCn0", IsDub = false,
					PosterID = db.Images.Single(i => i.Category == ImageCategory.Posters && i.Name == "Killers").ID,
					RateID = db.Rates.Single(r => r.Code == "PG-13").ID,
					Genres = new List<MovGen>()
					{
						new MovGen { GenreID = db.Genres.Single(g => g.Name == "Action").ID },
						new MovGen { GenreID = db.Genres.Single(g => g.Name == "Comedy").ID }
					}
				},
				new Movie
				{
					Title = "So undercover", Release = DateTime.Parse("2012-12-06"), Time = 94,
					Director = "Tom Vaughan", Actors = "Miley Cyrus, Jeremy Piven, Mike O'Malley",
					Plot = "A tough, street-smart private eye is hired by the FBI to go undercover in a college sorority.",
					Trailer = "SxvIWjEsCfA", IsDub = false,
					PosterID = db.Images.Single(i => i.Category == ImageCategory.Posters && i.Name == "So Undercover").ID,
					RateID = db.Rates.Single(r => r.Code == "PG-13").ID,
					Genres = new List<MovGen>()
					{
						new MovGen { GenreID = db.Genres.Single(g => g.Name == "Action").ID },
						new MovGen { GenreID = db.Genres.Single(g => g.Name == "Comedy").ID }
					}
				},
				new Movie
				{
					Title = "The machine", Release = DateTime.Parse("2014-03-21"), Time = 91,
					Director = "Caradog James", Actors = "Toby Stephens, Caity Lotz, Denis Lawson",
					Plot = "In efforts to construct perfect android killing machines in a war against China, UK scientists exceed their goal and create a sentient robot.",
					Trailer = "qVadlANZ9wU", IsDub = false,
					PosterID = db.Images.Single(i => i.Category == ImageCategory.Posters && i.Name == "The Machine").ID,
					RateID = db.Rates.Single(r => r.Code == "R").ID,
					Genres = new List<MovGen>()
					{
						new MovGen { GenreID = db.Genres.Single(g => g.Name == "Action").ID },
						new MovGen { GenreID = db.Genres.Single(g => g.Name == "Drama").ID },
						new MovGen { GenreID = db.Genres.Single(g => g.Name == "Sci-Fi").ID }
					}
				},
				new Movie
				{
					Title = "The nut job", Release = DateTime.Parse("2014-07-03"), Time = 85,
					Director = "Peter Lepeniotis", Actors = "Will Arnett, Brendan Fraser, Liam Neeson",
					Plot = "An incorrigibly self-serving exiled squirrel finds himself helping his former park brethren survive by raiding a nut store, a location that also happens to be a front for a human gang's bank robbery.",
					Trailer = "fq4qP2oSpIA", IsDub = false,
					PosterID = db.Images.Single(i => i.Category == ImageCategory.Posters && i.Name == "The Nut Job").ID,
					RateID = db.Rates.Single(r => r.Code == "G").ID,
					Genres = new List<MovGen>()
					{
						new MovGen { GenreID = db.Genres.Single(g => g.Name == "Animation").ID },
						//Adventure
						new MovGen { GenreID = db.Genres.Single(g => g.Name == "Comedy").ID }
					}
				},
				new Movie
				{
					Title = "The purge: anarchy", Release = DateTime.Parse("2014-07-17"), Time = 103,
					Director = "James DeMonaco", Actors = "Frank Grillo, Carmen Ejogo, Zach Gilford",
					Plot = "Three groups of people intertwine and are left stranded in the streets on Purge Night, trying to survive the chaos and violence that occurs.",
					Trailer = "Ec8T3drWnwM", IsDub = false,
					PosterID = db.Images.Single(i => i.Category == ImageCategory.Posters && i.Name == "The Purge Anarchy").ID,
					RateID = db.Rates.Single(r => r.Code == "R").ID,
					Genres = new List<MovGen>()
					{
						new MovGen { GenreID = db.Genres.Single(g => g.Name == "Action").ID },
						new MovGen { GenreID = db.Genres.Single(g => g.Name == "Horror").ID },
						new MovGen { GenreID = db.Genres.Single(g => g.Name == "Sci-Fi").ID }
					}
				},
				new Movie
				{
					Title = "Time lapse", Release = DateTime.Parse("2015-05-15"), Time = 104,
					Director = "Bradley King", Actors = "Danielle Panabaker, Matt O'Leary, George Finn",
					Plot = "Three friends discover a mysterious machine that takes pictures twenty-four hours into the future, and conspire to use it for personal gain, until disturbing and dangerous images begin to develop.",
					Trailer = "_YhP-VfH81E", IsDub = false,
					PosterID = db.Images.Single(i => i.Category == ImageCategory.Posters && i.Name == "Time Lapse").ID,
					RateID = db.Rates.Single(r => r.Code == "R").ID,
					Genres = new List<MovGen>()
					{
						new MovGen { GenreID = db.Genres.Single(g => g.Name == "Sci-Fi").ID },
						new MovGen { GenreID = db.Genres.Single(g => g.Name == "Thriller").ID }
					}
				},
				new Movie
				{
					Title = "Vehicle 19", Release = DateTime.Parse("2013-10-24"), Time = 82,
					Director = "Mukunda Michael Dewil", Actors = "Paul Walker, Naima McLean, Gys de Villiers",
					Plot = "In Johannesburg, an American parole breaker unknowingly picks up a rental car that will tie him to a web of corrupt local police.",
					Trailer = "CcHjfaAyI44", IsDub = false,
					PosterID = db.Images.Single(i => i.Category == ImageCategory.Posters && i.Name == "Vehicle 19").ID,
					RateID = db.Rates.Single(r => r.Code == "R").ID,
					Genres = new List<MovGen>()
					{
						new MovGen { GenreID = db.Genres.Single(g => g.Name == "Action").ID },
						//Crime
						new MovGen { GenreID = db.Genres.Single(g => g.Name == "Thriller").ID }
					}
				},
				new Movie
				{
					Title = "Walk of shame", Release = DateTime.Parse("2014-05-01"), Time = 95,
					Director = "Steven Brill", Actors = "Elizabeth Banks, James Marsden, Gillian Jacobs",
					Plot = "A reporter's dream of becoming a news anchor is compromised after a one-night stand leaves her stranded in downtown L.A. without a phone, car, ID or money - and only 8 hours to make it to the most important job interview of her life.",
					Trailer = "RTHZEFo7JsY", IsDub = false,
					PosterID = db.Images.Single(i => i.Category == ImageCategory.Posters && i.Name == "Walk Of Shame").ID,
					RateID = db.Rates.Single(r => r.Code == "R").ID,
					Genres = new List<MovGen>()
					{
						new MovGen { GenreID = db.Genres.Single(g => g.Name == "Comedy").ID }
					}
				},
				new Movie
				{
					Title = "We're the millers", Release = DateTime.Parse("2013-08-15"), Time = 110,
					Director = "Rawson Marshall Thurber", Actors = "Jason Sudeikis, Jennifer Aniston, Emma Roberts",
					Plot = "A veteran pot dealer creates a fake family as part of his plan to move a huge shipment of weed into the U.S. from Mexico.",
					Trailer = "A0Rac0iTui4", IsDub = false,
					PosterID = db.Images.Single(i => i.Category == ImageCategory.Posters && i.Name == "Were The Millers").ID,
					RateID = db.Rates.Single(r => r.Code == "R").ID,
					Genres = new List<MovGen>()
					{
						new MovGen { GenreID = db.Genres.Single(g => g.Name == "Comedy").ID }
						//Crime
					}
				},
				new Movie
				{
					Title = "White house down", Release = DateTime.Parse("2013-06-27"), Time = 131,
					Director = "Roland Emmerich", Actors = "Channing Tatum, Jamie Foxx, Maggie Gyllenhaal",
					Plot = "While on a tour of the White House with his young daughter, a Capitol policeman springs into action to save his child and protect the president from a heavily armed group of paramilitary invaders.",
					Trailer = "WfaTlmYvTA8", IsDub = false,
					PosterID = db.Images.Single(i => i.Category == ImageCategory.Posters && i.Name == "White House Down").ID,
					RateID = db.Rates.Single(r => r.Code == "PG-13").ID,
					Genres = new List<MovGen>()
					{
						new MovGen { GenreID = db.Genres.Single(g => g.Name == "Action").ID },
						new MovGen { GenreID = db.Genres.Single(g => g.Name == "Drama").ID },
						new MovGen { GenreID = db.Genres.Single(g => g.Name == "Thriller").ID }
					}
				},
				new Movie
				{
					Title = "Woman in gold", Release = DateTime.Parse("2015-04-02"), Time = 109,
					Director = "Simon Curtis", Actors = "Helen Mirren, Ryan Reynolds, Daniel Brühl",
					Plot = "Maria Altmann, an octogenarian Jewish refugee, takes on the Austrian government to recover artwork she believes rightfully belongs to her family.",
					Trailer = "9bx3KTGBEaI", IsDub = false,
					PosterID = db.Images.Single(i => i.Category == ImageCategory.Posters && i.Name == "Woman In Gold").ID,
					RateID = db.Rates.Single(r => r.Code == "PG-13").ID,
					Genres = new List<MovGen>()
					{
						new MovGen { GenreID = db.Genres.Single(g => g.Name == "Drama").ID }
						//Biography, History
					}
				},
				//Coming soon
				new Movie
				{
					Title = "Last vegas", Release = DateTime.Now.AddDays(10), Time = 105, //DateTime.Parse("2013-10-31")
					Director = "Jon Turteltaub", Actors = "Robert De Niro, Michael Douglas, Morgan Freeman",
					Plot = "Four friends take a break from their day-to-day lives to throw a bachelor party in Las Vegas for their last remaining single pal.",
					Trailer = "TvK3m0wJutI", IsDub = false,
					PosterID = db.Images.Single(i => i.Category == ImageCategory.Posters && i.Name == "Last Vegas").ID,
					RateID = db.Rates.Single(r => r.Code == "PG-13").ID,
					Genres = new List<MovGen>()
					{
						new MovGen { GenreID = db.Genres.Single(g => g.Name == "Comedy").ID }
					}
				},
				new Movie
				{
					Title = "Rio Dubbed", Release = DateTime.Now.AddDays(10), Time = 96, //DateTime.Parse("2011-04-07")
					Director = "Carlos Saldanha", Actors = "Jesse Eisenberg, Anne Hathaway, George Lopez",
					Plot = "When Blu, a domesticated macaw from small-town Minnesota, meets the fiercely independent Jewel, he takes off on an adventure to Rio de Janeiro with the bird of his dreams.",
					Trailer = "lDsvbki-3IM", IsDub = true,
					PosterID = db.Images.Single(i => i.Category == ImageCategory.Posters && i.Name == "Rio").ID,
					RateID = db.Rates.Single(r => r.Code == "G").ID,
					Genres = new List<MovGen>()
					{
						new MovGen { GenreID = db.Genres.Single(g => g.Name == "Animation").ID },
						//Adventure
						new MovGen { GenreID = db.Genres.Single(g => g.Name == "Comedy").ID }
					}
				},
				new Movie
				{
					Title = "The hobbit: the desolation of smaug", Release = DateTime.Now.AddDays(10), Time = 161, //DateTime.Parse("2013-12-12")
					Director = "Peter Jackson", Actors = "Ian McKellen, Martin Freeman, Richard Armitage",
					Plot = "The dwarves, along with Bilbo Baggins and Gandalf the Grey, continue their quest to reclaim Erebor, their homeland, from Smaug. Bilbo Baggins is in possession of a mysterious and magical ring.",
					Trailer = "OPVWy1tFXuc", IsDub = false,
					PosterID = db.Images.Single(i => i.Category == ImageCategory.Posters && i.Name == "The Hobbit The Desolation Of Smaug").ID,
					RateID = db.Rates.Single(r => r.Code == "PG-13").ID,
					Genres = new List<MovGen>()
					{
						new MovGen { GenreID = db.Genres.Single(g => g.Name == "Sci-Fi").ID }
						//Adventure, Fantasy
					}
				},
				new Movie
				{
					Title = "Tracers", Release = DateTime.Now.AddDays(10), Time = 94, //DateTime.Parse("2015-03-20")
					Director = "Daniel Benmayor", Actors = "Taylor Lautner, Marie Avgeropoulos, Adam Rayner",
					Plot = "Wanted by the Chinese mafia, a New York City bike messenger escapes into the world of parkour after meeting a beautiful stranger.",
					Trailer = "yqpfHeXqbQ8", IsDub = false,
					PosterID = db.Images.Single(i => i.Category == ImageCategory.Posters && i.Name == "Tracers").ID,
					RateID = db.Rates.Single(r => r.Code == "PG-13").ID,
					Genres = new List<MovGen>()
					{
						new MovGen { GenreID = db.Genres.Single(g => g.Name == "Action").ID },
						//Crime
						new MovGen { GenreID = db.Genres.Single(g => g.Name == "Drama").ID }
					}
				},
				new Movie
				{
					Title = "Noah", Release = DateTime.Now.AddDays(10), Time = 138, //DateTime.Parse("2014-03-27")
					Director = "Darren Aronofsky", Actors = "Russell Crowe, Jennifer Connelly, Anthony Hopkins",
					Plot = "Noah is chosen by God to undertake a momentous mission before an apocalyptic flood cleanses the world.",
					Trailer = "tAPNXwdlL68", IsDub = false,
					PosterID = db.Images.Single(i => i.Category == ImageCategory.Posters && i.Name == "Noah").ID,
					RateID = db.Rates.Single(r => r.Code == "PG-13").ID,
					Genres = new List<MovGen>()
					{
						new MovGen { GenreID = db.Genres.Single(g => g.Name == "Action").ID },
						//Adventure
						new MovGen { GenreID = db.Genres.Single(g => g.Name == "Drama").ID }
					}
				},
				new Movie
				{
					Title = "Frozen", Release = DateTime.Now.AddDays(10), Time = 102, //DateTime.Parse("2013-11-28")
					Director = "Chris Buck, Jennifer Lee", Actors = "Kristen Bell, Idina Menzel, Jonathan Groff",
					Plot = "When the newly-crowned Queen Elsa accidentally uses her power to turn things into ice to curse her home in infinite winter, her sister Anna teams up with a mountain man, his playful reindeer, and a snowman to change the weather condition.",
					Trailer = "TbQm5doF_Uc", IsDub = false,
					PosterID = db.Images.Single(i => i.Category == ImageCategory.Posters && i.Name == "Frozen").ID,
					RateID = db.Rates.Single(r => r.Code == "PG").ID,
					Genres = new List<MovGen>()
					{
						new MovGen { GenreID = db.Genres.Single(g => g.Name == "Animation").ID },
						//Adventure
						new MovGen { GenreID = db.Genres.Single(g => g.Name == "Comedy").ID }
					}
				}
			};
			foreach (Movie movie in demoMovies)
			{
				db.Movies.Add(movie);
			}
			db.SaveChanges();
		}

		private void ShowSeed()
		{
			var demoShows = new Show[]
			{
				new Show
				{
					HallID = db.Halls.Single(h => h.Name == "Hall 1").ID,
					MovieID = db.Movies.Single(m => m.Title == "Cars Dubbed").ID,
					ShowDate = DateTime.Today, ShowTime = TimeSpan.Parse("19:00")
				},
				new Show
				{
					HallID = db.Halls.Single(h => h.Name == "Hall 1").ID,
					MovieID = db.Movies.Single(m => m.Title == "Killers").ID,
					ShowDate = DateTime.Today, ShowTime = TimeSpan.Parse("22:00")
				},
				new Show
				{
					HallID = db.Halls.Single(h => h.Name == "Hall 2").ID,
					MovieID = db.Movies.Single(m => m.Title == "So undercover").ID,
					ShowDate = DateTime.Today, ShowTime = TimeSpan.Parse("19:00")
				},
				new Show
				{
					HallID = db.Halls.Single(h => h.Name == "Hall 2").ID,
					MovieID = db.Movies.Single(m => m.Title == "The machine").ID,
					ShowDate = DateTime.Today, ShowTime = TimeSpan.Parse("22:00")
				},
				new Show
				{
					HallID = db.Halls.Single(h => h.Name == "Hall 3").ID,
					MovieID = db.Movies.Single(m => m.Title == "The nut job").ID,
					ShowDate = DateTime.Today, ShowTime = TimeSpan.Parse("19:00")
				},
				new Show
				{
					HallID = db.Halls.Single(h => h.Name == "Hall 3").ID,
					MovieID = db.Movies.Single(m => m.Title == "The purge: anarchy").ID,
					ShowDate = DateTime.Today, ShowTime = TimeSpan.Parse("22:00")
				},
				new Show
				{
					HallID = db.Halls.Single(h => h.Name == "Hall 4").ID,
					MovieID = db.Movies.Single(m => m.Title == "Time lapse").ID,
					ShowDate = DateTime.Today, ShowTime = TimeSpan.Parse("19:00")
				},
				new Show
				{
					HallID = db.Halls.Single(h => h.Name == "Hall 4").ID,
					MovieID = db.Movies.Single(m => m.Title == "Vehicle 19").ID,
					ShowDate = DateTime.Today, ShowTime = TimeSpan.Parse("22:00")
				},
				new Show
				{
					HallID = db.Halls.Single(h => h.Name == "Hall 1").ID,
					MovieID = db.Movies.Single(m => m.Title == "Walk of shame").ID,
					ShowDate = DateTime.Today.AddDays(1), ShowTime = TimeSpan.Parse("19:00")
				},
				new Show
				{
					HallID = db.Halls.Single(h => h.Name == "Hall 1").ID,
					MovieID = db.Movies.Single(m => m.Title == "We're the millers").ID,
					ShowDate = DateTime.Today.AddDays(1), ShowTime = TimeSpan.Parse("22:00")
				},
				new Show
				{
					HallID = db.Halls.Single(h => h.Name == "Hall 2").ID,
					MovieID = db.Movies.Single(m => m.Title == "White house down").ID,
					ShowDate = DateTime.Today.AddDays(1), ShowTime = TimeSpan.Parse("19:30")
				},
				new Show
				{
					HallID = db.Halls.Single(h => h.Name == "Hall 2").ID,
					MovieID = db.Movies.Single(m => m.Title == "Woman in gold").ID,
					ShowDate = DateTime.Today.AddDays(1), ShowTime = TimeSpan.Parse("22:30")
				},
				new Show
				{
					HallID = db.Halls.Single(h => h.Name == "Hall 3").ID,
					MovieID = db.Movies.Single(m => m.Title == "Cars Dubbed").ID,
					ShowDate = DateTime.Today.AddDays(1), ShowTime = TimeSpan.Parse("19:00")
				},
				new Show
				{
					HallID = db.Halls.Single(h => h.Name == "Hall 3").ID,
					MovieID = db.Movies.Single(m => m.Title == "Killers").ID,
					ShowDate = DateTime.Today.AddDays(1), ShowTime = TimeSpan.Parse("22:00")
				},
				new Show
				{
					HallID = db.Halls.Single(h => h.Name == "Hall 4").ID,
					MovieID = db.Movies.Single(m => m.Title == "So undercover").ID,
					ShowDate = DateTime.Today.AddDays(1), ShowTime = TimeSpan.Parse("19:30")
				},
				new Show
				{
					HallID = db.Halls.Single(h => h.Name == "Hall 4").ID,
					MovieID = db.Movies.Single(m => m.Title == "The machine").ID,
					ShowDate = DateTime.Today.AddDays(1), ShowTime = TimeSpan.Parse("22:30")
				},
				new Show
				{
					HallID = db.Halls.Single(h => h.Name == "Hall 1").ID,
					MovieID = db.Movies.Single(m => m.Title == "Cars Dubbed").ID,
					ShowDate = DateTime.Today.AddDays(1), ShowTime = TimeSpan.Parse("16:00")
				},
				new Show
				{
					HallID = db.Halls.Single(h => h.Name == "Hall 1").ID,
					MovieID = db.Movies.Single(m => m.Title == "Cars Dubbed").ID,
					ShowDate = DateTime.Today.AddDays(2), ShowTime = TimeSpan.Parse("19:00")
				},
				new Show
				{
					HallID = db.Halls.Single(h => h.Name == "Hall 1").ID,
					MovieID = db.Movies.Single(m => m.Title == "Cars Dubbed").ID,
					ShowDate = DateTime.Today.AddDays(2), ShowTime = TimeSpan.Parse("21:00")
				},
				new Show
				{
					HallID = db.Halls.Single(h => h.Name == "Hall 2").ID,
					MovieID = db.Movies.Single(m => m.Title == "The nut job").ID,
					ShowDate = DateTime.Today.AddDays(2), ShowTime = TimeSpan.Parse("19:00")
				},
				new Show
				{
					HallID = db.Halls.Single(h => h.Name == "Hall 3").ID,
					MovieID = db.Movies.Single(m => m.Title == "Time lapse").ID,
					ShowDate = DateTime.Today.AddDays(2), ShowTime = TimeSpan.Parse("19:00")
				},
				new Show
				{
					HallID = db.Halls.Single(h => h.Name == "Hall 4").ID,
					MovieID = db.Movies.Single(m => m.Title == "Cars Dubbed").ID,
					ShowDate = DateTime.Today.AddDays(2), ShowTime = TimeSpan.Parse("19:00")
				},
				new Show
				{
					HallID = db.Halls.Single(h => h.Name == "Hall 1").ID,
					MovieID = db.Movies.Single(m => m.Title == "Cars Dubbed").ID,
					ShowDate = DateTime.Today.AddDays(3), ShowTime = TimeSpan.Parse("19:00")
				},
				new Show
				{
					HallID = db.Halls.Single(h => h.Name == "Hall 2").ID,
					MovieID = db.Movies.Single(m => m.Title == "The nut job").ID,
					ShowDate = DateTime.Today.AddDays(3), ShowTime = TimeSpan.Parse("22:30")
				},
				new Show
				{
					HallID = db.Halls.Single(h => h.Name == "Hall 3").ID,
					MovieID = db.Movies.Single(m => m.Title == "Time lapse").ID,
					ShowDate = DateTime.Today.AddDays(3), ShowTime = TimeSpan.Parse("22:00")
				},
				new Show
				{
					HallID = db.Halls.Single(h => h.Name == "Hall 4").ID,
					MovieID = db.Movies.Single(m => m.Title == "Cars Dubbed").ID,
					ShowDate = DateTime.Today.AddDays(3), ShowTime = TimeSpan.Parse("19:30")
				},
				new Show
				{
					HallID = db.Halls.Single(h => h.Name == "Hall 1").ID,
					MovieID = db.Movies.Single(m => m.Title == "Cars Dubbed").ID,
					ShowDate = DateTime.Today.AddDays(4), ShowTime = TimeSpan.Parse("19:00")
				},
				new Show
				{
					HallID = db.Halls.Single(h => h.Name == "Hall 2").ID,
					MovieID = db.Movies.Single(m => m.Title == "The nut job").ID,
					ShowDate = DateTime.Today.AddDays(4), ShowTime = TimeSpan.Parse("22:30")
				},
				new Show
				{
					HallID = db.Halls.Single(h => h.Name == "Hall 3").ID,
					MovieID = db.Movies.Single(m => m.Title == "Time lapse").ID,
					ShowDate = DateTime.Today.AddDays(4), ShowTime = TimeSpan.Parse("22:00")
				},
				new Show
				{
					HallID = db.Halls.Single(h => h.Name == "Hall 4").ID,
					MovieID = db.Movies.Single(m => m.Title == "Cars Dubbed").ID,
					ShowDate = DateTime.Today.AddDays(4), ShowTime = TimeSpan.Parse("19:30")
				},
				new Show
				{
					HallID = db.Halls.Single(h => h.Name == "Hall 1").ID,
					MovieID = db.Movies.Single(m => m.Title == "Cars Dubbed").ID,
					ShowDate = DateTime.Today.AddDays(5), ShowTime = TimeSpan.Parse("19:00")
				},
				new Show
				{
					HallID = db.Halls.Single(h => h.Name == "Hall 2").ID,
					MovieID = db.Movies.Single(m => m.Title == "The nut job").ID,
					ShowDate = DateTime.Today.AddDays(5), ShowTime = TimeSpan.Parse("22:30")
				},
				new Show
				{
					HallID = db.Halls.Single(h => h.Name == "Hall 3").ID,
					MovieID = db.Movies.Single(m => m.Title == "Time lapse").ID,
					ShowDate = DateTime.Today.AddDays(5), ShowTime = TimeSpan.Parse("22:00")
				},
				new Show
				{
					HallID = db.Halls.Single(h => h.Name == "Hall 4").ID,
					MovieID = db.Movies.Single(m => m.Title == "Cars Dubbed").ID,
					ShowDate = DateTime.Today.AddDays(5), ShowTime = TimeSpan.Parse("19:30")
				},
				new Show
				{
					HallID = db.Halls.Single(h => h.Name == "Hall 1").ID,
					MovieID = db.Movies.Single(m => m.Title == "Cars Dubbed").ID,
					ShowDate = DateTime.Today.AddDays(6), ShowTime = TimeSpan.Parse("19:00")
				},
				new Show
				{
					HallID = db.Halls.Single(h => h.Name == "Hall 2").ID,
					MovieID = db.Movies.Single(m => m.Title == "The nut job").ID,
					ShowDate = DateTime.Today.AddDays(6), ShowTime = TimeSpan.Parse("22:30")
				},
				new Show
				{
					HallID = db.Halls.Single(h => h.Name == "Hall 3").ID,
					MovieID = db.Movies.Single(m => m.Title == "Time lapse").ID,
					ShowDate = DateTime.Today.AddDays(6), ShowTime = TimeSpan.Parse("22:00")
				},
				new Show
				{
					HallID = db.Halls.Single(h => h.Name == "Hall 4").ID,
					MovieID = db.Movies.Single(m => m.Title == "Cars Dubbed").ID,
					ShowDate = DateTime.Today.AddDays(6), ShowTime = TimeSpan.Parse("19:30")
				},
				new Show
				{
					HallID = db.Halls.Single(h => h.Name == "Hall 1").ID,
					MovieID = db.Movies.Single(m => m.Title == "Cars Dubbed").ID,
					ShowDate = DateTime.Today.AddDays(7), ShowTime = TimeSpan.Parse("19:00")
				},
				new Show
				{
					HallID = db.Halls.Single(h => h.Name == "Hall 2").ID,
					MovieID = db.Movies.Single(m => m.Title == "The nut job").ID,
					ShowDate = DateTime.Today.AddDays(7), ShowTime = TimeSpan.Parse("22:30")
				},
				new Show
				{
					HallID = db.Halls.Single(h => h.Name == "Hall 3").ID,
					MovieID = db.Movies.Single(m => m.Title == "Time lapse").ID,
					ShowDate = DateTime.Today.AddDays(7), ShowTime = TimeSpan.Parse("22:00")
				},
				new Show
				{
					HallID = db.Halls.Single(h => h.Name == "Hall 4").ID,
					MovieID = db.Movies.Single(m => m.Title == "Cars Dubbed").ID,
					ShowDate = DateTime.Today.AddDays(7), ShowTime = TimeSpan.Parse("19:30")
				}
			};
			foreach (Show show in demoShows)
			{
				db.Shows.Add(show);
			}
			db.SaveChanges();
		}

		private void ReviewSeed()
		{
			var demoReviews = new Review[]
			{
				new Review
				{
					Rating = 8, PostTime = DateTime.Now.AddDays(-3).AddHours(-2),
					Comment = "Great movie.",
					MovieID = db.Movies.Single(m => m.Title == "Cars Dubbed").ID,
					ClientID = db.Clients.Single(cl => cl.UserName == "ZionAy").Id
				},
				new Review
				{
					Rating = 10, PostTime = DateTime.Now.AddDays(-4).AddHours(1),
					Comment = "Cool action with comedy.",
					MovieID = db.Movies.Single(m => m.Title == "Killers").ID,
					ClientID = db.Clients.Single(cl => cl.UserName == "ZionAy").Id
				},
				new Review
				{
					Rating = 3, PostTime = DateTime.Now.AddDays(-3).AddHours(6),
					Comment = "Can be better.",
					MovieID = db.Movies.Single(m => m.Title == "So undercover").ID,
					ClientID = db.Clients.Single(cl => cl.UserName == "ZionAy").Id
				},
				new Review
				{
					Rating = 9, PostTime = DateTime.Now.AddDays(-6).AddHours(-7).AddMinutes(21),
					Comment = "Must watch.",
					MovieID = db.Movies.Single(m => m.Title == "Cars Dubbed").ID,
					ClientID = db.Clients.Single(cl => cl.UserName == "MrMovie").Id
				},
				new Review
				{
					Rating = 8, PostTime = DateTime.Now.AddDays(-2).AddHours(-5),
					Comment = "I will watch it again.",
					MovieID = db.Movies.Single(m => m.Title == "Killers").ID,
					ClientID = db.Clients.Single(cl => cl.UserName == "MrMovie").Id
				},
				new Review
				{
					Rating = 9, PostTime = DateTime.Now.AddDays(-2).AddHours(2),
					Comment = "Good director.",
					MovieID = db.Movies.Single(m => m.Title == "So undercover").ID,
					ClientID = db.Clients.Single(cl => cl.UserName == "MrMovie").Id
				},
				new Review
				{
					Rating = 3, PostTime = DateTime.Now.AddDays(-3).AddHours(-5),
					Comment = "Disapointment.",
					MovieID = db.Movies.Single(m => m.Title == "Cars Dubbed").ID,
					ClientID = db.Clients.Single(cl => cl.UserName == "HappyHippo").Id
				},
				new Review
				{
					Rating = 2, PostTime = DateTime.Now.AddDays(-6).AddHours(8),
					Comment = "Don't go.",
					MovieID = db.Movies.Single(m => m.Title == "Killers").ID,
					ClientID = db.Clients.Single(cl => cl.UserName == "HappyHippo").Id
				},
				new Review
				{
					Rating = 3, PostTime = DateTime.Now.AddDays(-1).AddHours(-1),
					Comment = "Too much drama for a comedy movie.",
					MovieID = db.Movies.Single(m => m.Title == "Walk of shame").ID,
					ClientID = db.Clients.Single(cl => cl.UserName == "PandaBear").Id
				},
				new Review
				{
					Rating = 9, PostTime = DateTime.Now.AddDays(-3).AddHours(3).AddMinutes(20),
					Comment = "Take your friends to that movie!",
					MovieID = db.Movies.Single(m => m.Title == "Vehicle 19").ID,
					ClientID = db.Clients.Single(cl => cl.UserName == "Scippy").Id
				}
			};
			foreach (Review review in demoReviews)
			{
				db.Reviews.Add(review);
			}
			db.SaveChanges();
		}

		private void OrderSeed()
		{
			var demoOrders = new Order[]
			{
				new Order
				{
					ClientID = db.Clients.Single(cl => cl.UserName == "Scippy").Id,
					Payment = new Payment
					{
						FirstName = "Scippy", LastName = "Scippy", CCID = "123456789",
						CCNum = "1234123412341234", CCCVV = "123", CCMonth = 3, CCYear = 2019,
						//Address = "", City = "", ZipCode = "",
						SendInvoice = false
					},
					Tickets = new List<Ticket>()
					{
						new Ticket
						{
							ClientID = db.Clients.Single(cl => cl.UserName == "Scippy").Id,
							ShowID = db.Shows.Single(s => s.Hall.Name == "Hall 1" && s.Movie.Title == "Cars Dubbed" && s.ShowDate == DateTime.Today && s.ShowTime == TimeSpan.Parse("19:00")).ID,
							Row = 1, Col = 4
						},
						new Ticket
						{
							ClientID = db.Clients.Single(cl => cl.UserName == "Scippy").Id,
							ShowID = db.Shows.Single(s => s.Hall.Name == "Hall 1" && s.Movie.Title == "Cars Dubbed" && s.ShowDate == DateTime.Today && s.ShowTime == TimeSpan.Parse("19:00")).ID,
							Row = 1, Col = 5
						},
						new Ticket
						{
							ClientID = db.Clients.Single(cl => cl.UserName == "Scippy").Id,
							ShowID = db.Shows.Single(s => s.Hall.Name == "Hall 1" && s.Movie.Title == "Cars Dubbed" && s.ShowDate == DateTime.Today && s.ShowTime == TimeSpan.Parse("19:00")).ID,
							Row = 1, Col = 6
						},
						new Ticket
						{
							ClientID = db.Clients.Single(cl => cl.UserName == "Scippy").Id,
							ShowID = db.Shows.Single(s => s.Hall.Name == "Hall 1" && s.Movie.Title == "Cars Dubbed" && s.ShowDate == DateTime.Today && s.ShowTime == TimeSpan.Parse("19:00")).ID,
							Row = 1, Col = 7
						}
					}
				},
				new Order
				{
					ClientID = db.Clients.Single(cl => cl.UserName == "HappyHippo").Id,
					Payment = new Payment
					{
						FirstName = "Happy", LastName = "Hippo", CCID = "345678912",
						CCNum = "3456345634563456", CCCVV = "345", CCMonth = 6, CCYear = 2020,
						Address = "Happy street 345", City = "Happyland", ZipCode = "HIP3456",
						SendInvoice = true
					},
					Tickets = new List<Ticket>()
					{
						new Ticket
						{
							ClientID = db.Clients.Single(cl => cl.UserName == "HappyHippo").Id,
							ShowID = db.Shows.Single(s => s.Hall.Name == "Hall 2" && s.Movie.Title == "So undercover" && s.ShowDate == DateTime.Today && s.ShowTime == TimeSpan.Parse("19:00")).ID,
							Row = 3, Col = 2
						},
						new Ticket
						{
							ClientID = db.Clients.Single(cl => cl.UserName == "HappyHippo").Id,
							ShowID = db.Shows.Single(s => s.Hall.Name == "Hall 2" && s.Movie.Title == "So undercover" && s.ShowDate == DateTime.Today && s.ShowTime == TimeSpan.Parse("19:00")).ID,
							Row = 3, Col = 3
						},
						new Ticket
						{
							ClientID = db.Clients.Single(cl => cl.UserName == "HappyHippo").Id,
							ShowID = db.Shows.Single(s => s.Hall.Name == "Hall 2" && s.Movie.Title == "So undercover" && s.ShowDate == DateTime.Today && s.ShowTime == TimeSpan.Parse("19:00")).ID,
							Row = 3, Col = 4
						},
						new Ticket
						{
							ClientID = db.Clients.Single(cl => cl.UserName == "HappyHippo").Id,
							ShowID = db.Shows.Single(s => s.Hall.Name == "Hall 2" && s.Movie.Title == "So undercover" && s.ShowDate == DateTime.Today && s.ShowTime == TimeSpan.Parse("19:00")).ID,
							Row = 3, Col = 5
						}
					}
				},
				new Order
				{
					ClientID = db.Clients.Single(cl => cl.UserName == "PandaBear").Id,
					Payment = new Payment
					{
						FirstName = "Panda", LastName = "Bear", CCID = "567891234",
						CCNum = "5678567856785678", CCCVV = "567", CCMonth = 9, CCYear = 2019,
						//Address = "", City = "", ZipCode = "",
						SendInvoice = false
					},
					Tickets = new List<Ticket>()
					{
						new Ticket
						{
							ClientID = db.Clients.Single(cl => cl.UserName == "PandaBear").Id,
							ShowID = db.Shows.Single(s => s.Hall.Name == "Hall 1" && s.Movie.Title == "Cars Dubbed" && s.ShowDate == DateTime.Today && s.ShowTime == TimeSpan.Parse("19:00")).ID,
							Row = 5, Col = 6
						},
						new Ticket
						{
							ClientID = db.Clients.Single(cl => cl.UserName == "PandaBear").Id,
							ShowID = db.Shows.Single(s => s.Hall.Name == "Hall 1" && s.Movie.Title == "Cars Dubbed" && s.ShowDate == DateTime.Today && s.ShowTime == TimeSpan.Parse("19:00")).ID,
							Row = 5, Col = 7
						}
					}
				},
				new Order
				{
					ClientID = db.Clients.Single(cl => cl.UserName == "Scippy").Id,
					Payment = new Payment
					{
						FirstName = "Scippy", LastName = "Scippy", CCID = "123456789",
						CCNum = "1234123412341234", CCCVV = "123", CCMonth = 3, CCYear = 2019,
						//Address = "", City = "", ZipCode = "",
						SendInvoice = false
					},
					Tickets = new List<Ticket>()
					{
						new Ticket
						{
							ClientID = db.Clients.Single(cl => cl.UserName == "Scippy").Id,
							ShowID = db.Shows.Single(s => s.Hall.Name == "Hall 1" && s.Movie.Title == "Walk of shame" && s.ShowDate == DateTime.Today.AddDays(1) && s.ShowTime == TimeSpan.Parse("19:00")).ID,
							Row = 8, Col = 5
						},
						new Ticket
						{
							ClientID = db.Clients.Single(cl => cl.UserName == "Scippy").Id,
							ShowID = db.Shows.Single(s => s.Hall.Name == "Hall 1" && s.Movie.Title == "Walk of shame" && s.ShowDate == DateTime.Today.AddDays(1) && s.ShowTime == TimeSpan.Parse("19:00")).ID,
							Row = 8, Col = 6
						},
						new Ticket
						{
							ClientID = db.Clients.Single(cl => cl.UserName == "Scippy").Id,
							ShowID = db.Shows.Single(s => s.Hall.Name == "Hall 1" && s.Movie.Title == "Walk of shame" && s.ShowDate == DateTime.Today.AddDays(1) && s.ShowTime == TimeSpan.Parse("19:00")).ID,
							Row = 8, Col = 7
						}
					}
				},
				new Order
				{
					ClientID = db.Clients.Single(cl => cl.UserName == "HappyHippo").Id,
					Payment = new Payment
					{
						FirstName = "Happy", LastName = "Hippo", CCID = "345678912",
						CCNum = "3456345634563456", CCCVV = "345", CCMonth = 6, CCYear = 2020,
						//Address = "", City = "", ZipCode = "",
						SendInvoice = false
					},
					Tickets = new List<Ticket>()
					{
						new Ticket
						{
							ClientID = db.Clients.Single(cl => cl.UserName == "HappyHippo").Id,
							ShowID = db.Shows.Single(s => s.Hall.Name == "Hall 1" && s.Movie.Title == "Walk of shame" && s.ShowDate == DateTime.Today.AddDays(1) && s.ShowTime == TimeSpan.Parse("19:00")).ID,
							Row = 9, Col = 1
						},
						new Ticket
						{
							ClientID = db.Clients.Single(cl => cl.UserName == "HappyHippo").Id,
							ShowID = db.Shows.Single(s => s.Hall.Name == "Hall 1" && s.Movie.Title == "Walk of shame" && s.ShowDate == DateTime.Today.AddDays(1) && s.ShowTime == TimeSpan.Parse("19:00")).ID,
							Row = 9, Col = 2
						},
						new Ticket
						{
							ClientID = db.Clients.Single(cl => cl.UserName == "HappyHippo").Id,
							ShowID = db.Shows.Single(s => s.Hall.Name == "Hall 1" && s.Movie.Title == "Walk of shame" && s.ShowDate == DateTime.Today.AddDays(1) && s.ShowTime == TimeSpan.Parse("19:00")).ID,
							Row = 9, Col = 3
						},
						new Ticket
						{
							ClientID = db.Clients.Single(cl => cl.UserName == "HappyHippo").Id,
							ShowID = db.Shows.Single(s => s.Hall.Name == "Hall 1" && s.Movie.Title == "Walk of shame" && s.ShowDate == DateTime.Today.AddDays(1) && s.ShowTime == TimeSpan.Parse("19:00")).ID,
							Row = 9, Col = 4
						}
					}
				},
				new Order
				{
					ClientID = db.Clients.Single(cl => cl.UserName == "PandaBear").Id,
					Payment = new Payment
					{
						FirstName = "Panda", LastName = "Bear", CCID = "567891234",
						CCNum = "5678567856785678", CCCVV = "567", CCMonth = 9, CCYear = 2019,
						//Address = "", City = "", ZipCode = "",
						SendInvoice = false
					},
					Tickets = new List<Ticket>()
					{
						new Ticket
						{
							ClientID = db.Clients.Single(cl => cl.UserName == "PandaBear").Id,
							ShowID = db.Shows.Single(s => s.Hall.Name == "Hall 1" && s.Movie.Title == "We're the millers" && s.ShowDate == DateTime.Today.AddDays(1) && s.ShowTime == TimeSpan.Parse("22:00")).ID,
							Row = 5, Col = 4
						},
						new Ticket
						{
							ClientID = db.Clients.Single(cl => cl.UserName == "Scippy").Id,
							ShowID = db.Shows.Single(s => s.Hall.Name == "Hall 1" && s.Movie.Title == "We're the millers" && s.ShowDate == DateTime.Today.AddDays(1) && s.ShowTime == TimeSpan.Parse("22:00")).ID,
							Row = 5, Col = 5
						}
					}
				}
			};
			foreach (Order order in demoOrders)
			{
				db.Orders.Add(order);
			}
			db.SaveChanges();
		}
	}
}
