using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Cinematicks.Models
{
    public class Rate
    {
		public int ID { get; set; }
		
		[Required(AllowEmptyStrings = false, ErrorMessage = "This field is required.")]
		[StringLength(15, MinimumLength = 1, ErrorMessage = "Rate code must be 1-15 characters.")]
		[Display(Name = "Rate Code")]
		public string Code { get; set; }

		[Required(AllowEmptyStrings = false, ErrorMessage = "This field is required.")]
		[StringLength(150, MinimumLength = 2, ErrorMessage = "Rate description must be 2-30 characters.")]
		[Display(Name = "Description")]
		public string Description { get; set; }

		[Required(ErrorMessage = "This field is required.")]
		[Range(0, 21, ErrorMessage = "Minimum age must be 0-21.")]
		[Display(Name = "Minimum Age")]
		public int MinAge { get; set; }

		[Required(ErrorMessage = "This field is required.")]
		[Display(Name = "Rate Image")]
		public int ImageID { get; set; }


		public virtual Image Image { get; set; }
		public virtual ICollection<Movie> Movies { get; set; }
	}
}
