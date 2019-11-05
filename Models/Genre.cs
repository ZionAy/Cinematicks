using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Cinematicks.Models
{
    public class Genre
    {
		public int ID { get; set; }

		[Required(AllowEmptyStrings = false, ErrorMessage = "This field is required.")]
		[StringLength(20, MinimumLength = 2, ErrorMessage = "Genre name must be 2-20 characters.")]
		[Display(Name = "Name")]
		public string Name { get; set; }

		[StringLength(100, ErrorMessage = "Genre description must be up to 100 characters.")]
		[Display(Name = "Description")]
		public string Description { get; set; }

		[Required(ErrorMessage = "This field is required.")]
		[Display(Name = "Show in menu")]
		public bool InMenu { get; set; } = false;


		public virtual ICollection<MovGen> Movies { get; set; }
	}
}
