using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Cinematicks.Models
{
    public class PhotoGal
    {
		public int ID { get; set; }

		[Required(AllowEmptyStrings = false, ErrorMessage = "This field is required.")]
		[StringLength(30, MinimumLength = 2, ErrorMessage = "Photo title must be 2-30 characters.")]
		[Display(Name = "Title")]
		public string Title { get; set; }

		[StringLength(150, MinimumLength = 2, ErrorMessage = "Photo description must be 2-150 characters.")]
		[Display(Name = "Description")]
		public string Description { get; set; }

		[Required(ErrorMessage = "This field is required.")]
		[Display(Name = "Active Photo")]
		public bool IsActive { get; set; } = false;

		[Required(ErrorMessage = "This field is required.")]
		[Display(Name = "Photo Image")]
		public int PhotoID { get; set; }

		[Required(ErrorMessage = "This field is required.")]
		[Display(Name = "For Cinema")]
		public int CinemaID { get; set; }


		public virtual Image Photo { get; set; }
		public virtual Cinema Cinema { get; set; }
	}
}
