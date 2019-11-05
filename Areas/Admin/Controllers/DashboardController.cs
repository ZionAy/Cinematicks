using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Cinematicks.Models;
using Cinematicks.ViewModels;

namespace Cinematicks.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class DashboardController : Controller
    {
        private readonly DBContext db;
        public DashboardController(DBContext context)
        {
            db = context;
        }

		[Route("Admin")] // GET: Admin
		[Route("Admin/Index")] // GET: Admin/Index
		[Route("Admin/Dashboard")] // GET: Admin/Dashboard
		public async Task<IActionResult> Index()
		{
			var model = new DashViewModel(db);
			await model.Init();
			return View(model);
		}
    }
}
