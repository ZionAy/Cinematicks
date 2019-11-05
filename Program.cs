using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Cinematicks.Data;
using Cinematicks.Models;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Cinematicks
{
    public class Program
    {
        public static void Main(string[] args)
        {
			/* Start DB Initialization on startup */
			var host = BuildWebHost(args);
			using (var scope = host.Services.CreateScope())
			{
				var services = scope.ServiceProvider;
				try
				{
					var context = services.GetRequiredService<DBContext>();
					var uManager = services.GetRequiredService<UserManager<Client>>();
					var rManager = services.GetRequiredService<RoleManager<IdentityRole>>();
					var demoData = new DemoData(context, uManager, rManager);
				}
				catch (Exception ex)
				{
					var logger = services.GetRequiredService<ILogger<Program>>();
					logger.LogError(ex, "An error occurred while seeding the database.");
					Console.WriteLine("An error occurred while seeding the database.");
				}
			}
			host.Run();
			/* End DB Initialization on startup */

			//Comment-ed out for the DB initialization on startup
			//If no DB init needed, delete the code of the init and uncomment this line!
			//BuildWebHost(args).Run();
		}

		public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .Build();
    }
}
