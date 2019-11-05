using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Cinematicks.Models
{
    public class Image
	{
		public int ID { get; set; }

		[Required(AllowEmptyStrings = false, ErrorMessage = "This field is required.")]
		[StringLength(80, MinimumLength = 2, ErrorMessage = "Image name must be 2-80 characters.")]
		[Display(Name = "Name")]
		public string Name { get; set; }

		[Required(ErrorMessage = "This field is required.")]
		[Display(Name = "Category")]
		public ImageCategory Category { get; set; }

		[NotMapped]
		[Required(ErrorMessage = "This field is required.")]
		[DataType(DataType.Upload)]
		[Display(Name = "Image")]
		public IFormFile Upload { get; set; }

		[DataType(DataType.ImageUrl)]
		[Display(Name = "Filename")]
		public string Filename { get; set; }

		[Column(TypeName = "smalldatetime")]
		[Display(Name = "Uploaded at")]
		public DateTime UploadedTime { get; set; } = DateTime.Now;

		[NotMapped]
		public string FilePath
		{
			get { return this.Category.ToString() + "/" + this.Filename; }
		}


		public virtual ICollection<Client> Avatars { get; set; }
		public virtual ICollection<Cinema> Cinemas { get; set; }
		public virtual ICollection<Rate> Rates { get; set; }
		public virtual ICollection<Location> Locations { get; set; }
		public virtual ICollection<PhotoGal> Gallery { get; set; }
		public virtual ICollection<Movie> Movies { get; set; }
		public virtual ICollection<Promo> Banners { get; set; }
	}

	public enum ImageCategory
	{
		Avatars,
		Banners,
		Gallery,
		Maps,
		Rates,
		Posters
	}
}
