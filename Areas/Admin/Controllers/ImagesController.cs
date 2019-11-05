using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Cinematicks.Models;
using Microsoft.AspNetCore.Authorization;
using System.IO;
using Cinematicks.ViewModels;

namespace Cinematicks.Areas.Admin.Controllers
{
    [Area("Admin")]
	[Authorize(Roles = "Admin")]
	public class ImagesController : Controller
    {
        private readonly DBContext db;
        public ImagesController(DBContext context)
        {
            db = context;
        }

		/* List of uploaded images */
		public async Task<IActionResult> Index()
		{
			var dbImages = db.Images;
			ViewData["Categories"] = new SelectList(GetCatEnumList(), "Value", "Text");
			return View(await dbImages.ToListAsync());
		}

		/* List of uploaded images after being filtered */
		[HttpPost]
		public async Task<IActionResult> Index(string find, ImageCategory? cat)
		{
			ViewData["Categories"] = new SelectList(GetCatEnumList(), "Value", "Text");
			if (find == null)
			{
				if (cat == null || Enum.GetName(typeof(ImageCategory), cat) == null)
				{
					return RedirectToAction(nameof(Index));
				}
				else
				{
					var dbImages = db.Images
						.Where(i => i.Category == cat);
					return View(await dbImages.ToListAsync());
				}
			}
			else
			{
				find = find.Trim();
				if (cat == null || Enum.GetName(typeof(ImageCategory), cat) == null)
				{
					var dbImages = db.Images
						.Where(i => i.Name.Contains(find));
					return View(await dbImages.ToListAsync());
				}
				else
				{
					var dbImages = db.Images
						.Where(i => i.Category == cat && i.Name.Contains(find));
					return View(await dbImages.ToListAsync());
				}
			}
		}

		/* Info of an uploaded image */
		public async Task<IActionResult> Info(int? id)
		{
			if (id == null) { return RedirectToAction("Page404", "Home"); }
			var dbImage = await db.Images
				.SingleOrDefaultAsync(i => i.ID == id);
			if (dbImage == null) { return RedirectToAction("Page404", "Home"); }
			return View(dbImage);
		}


		/* Upload new image file */
		public IActionResult Upload()
		{
			ViewData["Categories"] = new SelectList(GetCatEnumList(), "Value", "Text");
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Upload([Bind("Name,Category,Upload")] Image img)
		{
			ImageDBExist(img.Name, img.Category);
			if (ModelState.IsValid)
			{
				// Save the image on server
				if ((img.Upload.Length > 0) && (IsImage(Path.GetExtension(img.Upload.FileName))))
				{
					img.Filename = Guid.NewGuid().ToString() + Path.GetExtension(img.Upload.FileName);
					var savePath = Path.Combine("wwwroot/images/", img.Category.ToString(), img.Filename);
					using (var stream = new FileStream(savePath, FileMode.Create))
					{
						img.Upload.CopyTo(stream);
					}
				}
				else
				{
					ModelState.AddModelError("Upload", "Please choose an image file.");
					ViewData["Categories"] = new SelectList(GetCatEnumList(), "Value", "Text");
					return View(img);
				}
				try
				{
					db.Images.Add(img);
					await db.SaveChangesAsync();
					TempData["Status"] = "A new image named " + img.Name + " has been added to " + img.Category.ToString() + "category.";
					TempData["Color"] = "success";
					return RedirectToAction(nameof(Index));
				}
				catch (Exception)
				{
					TempData["Status"] = "We're sorry, an unexpected error has been accured." + Environment.NewLine + "If you keep getting this error please contact system administrator.";
					TempData["Color"] = "danger";
					ViewData["Categories"] = new SelectList(GetCatEnumList(), "Value", "Text");
					return View(img);
				}
			}
			ViewData["Categories"] = new SelectList(GetCatEnumList(), "Value", "Text");
			return View(img);
		}


		/* Edit the uploaded image */
		public async Task<IActionResult> Edit(int? id)
		{
			if (id == null) { return RedirectToAction("Page404", "Home"); }
			var dbImage = await db.Images
				.SingleOrDefaultAsync(i => i.ID == id);
			if (dbImage == null) { return RedirectToAction("Page404", "Home"); }

			// ViewModel used to change only specific details and file is not required
			var editImage = new EditImageViewModel
			{
				ID = dbImage.ID,
				Name = dbImage.Name,
				Category = dbImage.Category
			};
			ViewData["Categories"] = new SelectList(GetCatEnumList(), "Value", "Text");
			return View(editImage);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(int id, [Bind("ID,Name,Category,Upload")] EditImageViewModel editImage)
		{
			if (id != editImage.ID)
			{
				TempData["Status"] = "We're sorry, an unexpected error has been accured." + Environment.NewLine + "If you keep getting this error please contact system administrator.";
				TempData["Color"] = "danger";
				return RedirectToAction(nameof(Index));
			}
			ImageDBExist(editImage.Name, editImage.Category, editImage.ID);
			var dbImage = await db.Images
				.SingleOrDefaultAsync(i => i.ID == id);
			// Check if name+category already exists
			//if (dbImage.Name != editImage.Name || dbImage.Category != editImage.Category)
			//{
			//	var existImage = await db.Images
			//		.SingleOrDefaultAsync(i => i.Name == editImage.Name && i.Category == editImage.Category);
			//	if (existImage != null)
			//	{
			//		ModelState.AddModelError("Name", "Image with that name already exists in this category.");
			//	}
			//}
			if (ModelState.IsValid)
			{
				var oldFile = Path.Combine("wwwroot/images/", dbImage.Category.ToString(), dbImage.Filename);
				if (editImage.Upload != null)
				{
					// Check if new file has been chosen to be uploaded
					if (editImage.Upload.Length > 0 && IsImage(Path.GetExtension(editImage.Upload.FileName)))
					{
						dbImage.Filename = Guid.NewGuid().ToString() + Path.GetExtension(editImage.Upload.FileName);
						var savePath = Path.Combine("wwwroot/images/", editImage.Category.ToString(), dbImage.Filename);
						using (var stream = new FileStream(savePath, FileMode.Create))
						{
							// Upload the new file to the server
							editImage.Upload.CopyTo(stream);
							// Delete the old file from the server
							if (System.IO.File.Exists(oldFile))
							{
								System.IO.File.Delete(oldFile);
							}
						}
					}
					else
					{
						ModelState.AddModelError("Upload", "Please choose an image file.");
						ViewData["Categories"] = new SelectList(GetCatEnumList(), "Value", "Text");
						return View(editImage);
					}
				}
				dbImage.Name = editImage.Name;
				dbImage.Category = editImage.Category;
				try
				{
					db.Images.Update(dbImage);
					await db.SaveChangesAsync();
					TempData["Status"] = "The changes you made to the image have been saved!";
					TempData["Color"] = "success";
				}
				catch (DbUpdateConcurrencyException)
				{
					if (!ImageExists(dbImage.ID)) { return RedirectToAction("Page404", "Home"); }
					else
					{
						TempData["Status"] = "We're sorry, an unexpected error has been accured." + Environment.NewLine + "If you keep getting this error please contact system administrator.";
						TempData["Color"] = "danger";
						return RedirectToAction(nameof(Index));
					}
				}
				return RedirectToAction(nameof(Info), new { id = dbImage.ID });
			}
			ViewData["Categories"] = new SelectList(GetCatEnumList(), "Value", "Text");
			return View(editImage);
		}


		/* Delete image file */
		public async Task<IActionResult> Delete(int? id)
		{
			if (id == null) { return RedirectToAction("Page404", "Home"); }
			var dbImage = await db.Images
				.SingleOrDefaultAsync(i => i.ID == id);
			if (dbImage == null) { return RedirectToAction("Page404", "Home"); }
			return View(dbImage);
		}

		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(int id)
		{
			var dbImage = await db.Images
				.SingleOrDefaultAsync(i => i.ID == id);
			if (dbImage == null) { return RedirectToAction("Page404", "Home"); }
			try
			{
				// Delete the file from server
				var filePath = Path.Combine("wwwroot/images/", dbImage.Category.ToString(), dbImage.Filename);
				if (System.IO.File.Exists(filePath))
				{
					System.IO.File.Delete(filePath);
				}
				db.Images.Remove(dbImage);
				await db.SaveChangesAsync();
				TempData["Status"] = "The image has been successfully deleted!";
				TempData["Color"] = "success";
			}
			catch (Exception)
			{
				TempData["Status"] = "We're sorry, an unexpected error has been accured." + Environment.NewLine + "If you keep getting this error please contact system administrator.";
				TempData["Color"] = "danger";
			}			
			return RedirectToAction(nameof(Index));
		}


		// Check if exists in DB
		private bool ImageExists(int id)
		{
			return db.Images.Any(e => e.ID == id);
		}

		/* Server Validations */
		private void ImageDBExist(string iName, ImageCategory iCat, int? id = null)
		{
			// Unique name and category
			var dbImage = db.Images.AsNoTracking().FirstOrDefault(i => i.Name == iName && i.Category == iCat);
			if ((dbImage != null && id == null) || (dbImage != null && dbImage.ID != id))
			{
				ModelState.AddModelError("Name", "An image with that name already exists on this category.");
			}
		}


		/* Custom get image for admin select preview */
		[HttpPost]
		public JsonResult GetImage(int id)
		{
			//int.TryParse(id, out int id);
			var dbImage = db.Images
				.SingleOrDefault(i => i.ID == id);
			return Json(new { File = dbImage.FilePath });
		}

		/* Other Custom methods */
		// Create select list of image categories
		private List<SelectListItem> GetCatEnumList()
		{
			List<SelectListItem> list = new List<SelectListItem>();
			var values = Enum.GetValues(typeof(ImageCategory));
			foreach (var item in values)
			{
				list.Add(new SelectListItem
				{
					Value = ((int)item).ToString(),
					Text = Enum.GetName(typeof(ImageCategory), item)
				});
			}
			return list;
		}

		// Check if the file has image extension
		private bool IsImage(string extension)
		{
			switch (extension)
			{
				case ".jpg":
				case ".jpeg":
				case ".png":
				case ".gif":
				case ".bmp":
					return true;
				default:
					return false;
			}
		}

	}
}
