using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Cinematicks.Data;
using Cinematicks.Models;
using Cinematicks.Services;

namespace Cinematicks
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
			//Add DBContext via dependecy injection
			services.AddDbContext<DBContext>(options =>
				options.UseSqlServer(Configuration.GetConnectionString("Development")));

			//Add Identity via dependency injection
			services.AddIdentity<Client, IdentityRole>()
				.AddEntityFrameworkStores<DBContext>()
				.AddDefaultTokenProviders();

			/* Identity configuration */
			services.Configure<IdentityOptions>(options =>
			{
				// Password configuration:
				// Minimum length: 8 characters, Requires at least 1 digit, Letters and special chars are not must.
				// At least 2 different characters. 
				options.Password.RequiredLength = 6;
				options.Password.RequireDigit = true;
				options.Password.RequireNonAlphanumeric = false;
				options.Password.RequireUppercase = false;
				options.Password.RequireLowercase = false;
				options.Password.RequiredUniqueChars = 2;

				// Lockout configuration: 
				// Failed tries allowed: 5, Lockout: 20 minutes.
				options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(20);
				options.Lockout.MaxFailedAccessAttempts = 5;

				// Signin - confirmation after register:
				// Confirmed Email: true.
				options.SignIn.RequireConfirmedEmail = true;
				
				// Username:
				// Client must have unique email. Allowed username chars: letters, digits, hyphen and underscore.
				options.User.RequireUniqueEmail = true;
				options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890-_";
			});

			/* Cookie configuration */
			services.ConfigureApplicationCookie(options =>
			{
				options.Cookie.Name = "CinemaTicks"; // Name of the cookie.
				options.Cookie.HttpOnly = true; // Not accessible from client-side scripts.
				options.ExpireTimeSpan = TimeSpan.FromDays(90); // Expiration time from the creation point.
															   
				options.LoginPath = "/Account/Login"; //Redirection path when user is unauthorized.
				options.LogoutPath = "/Account/Logout"; //Redirection path when user is logged out.
				options.AccessDeniedPath = "/Account/AccessDenied"; //Redirection path when user fails an authorization check.
				options.SlidingExpiration = true; //Issued a new cookie with a new expiration time if expiration passed halfway.
			});

			//Add application email services
			//services.AddTransient<IEmailSender, EmailSender>();
			services.AddTransient<IEmailSender, MyEmailService>();

			services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

			app.UseAuthentication(); //Identity middleware

			app.UseMvc(routes =>
            {
				routes.MapRoute(
					name: "areas",
					template: "{area:exists}/{controller=Home}/{action=Index}/{id?}");
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
