using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Cinematicks.Models
{
    public class Promo
    {
		public int ID { get; set; }

		[Required(AllowEmptyStrings = false, ErrorMessage = "This field is required.")]
		[StringLength(30, MinimumLength = 2, ErrorMessage = "Promo name must be 2-30 characters.")]
		[Display(Name = "Promo Name")]
		public string Name { get; set; }

		[Required(AllowEmptyStrings = false, ErrorMessage = "This field is required.")]
		[StringLength(800, MinimumLength = 5, ErrorMessage = "Promo description must be 5-800 characters.")]
		[DataType(DataType.MultilineText)]
		[Display(Name = "Description")]
		public string Description { get; set; }

		[Column(TypeName = "date")]
		[Required(ErrorMessage = "This field is required.")]
		[DataType(DataType.Date)]
		[Display(Name = "Starting On")]
		public DateTime StartTime { get; set; }

		[Column(TypeName = "date")]
		[Required(ErrorMessage = "This field is required.")]
		[DataType(DataType.Date)]
		[Display(Name = "Ending On")]
		public DateTime EndTime { get; set; }

		[Required(ErrorMessage = "This field is required.")]
		[Display(Name = "Deal Banner")]
		public int BannerID { get; set; }


		public virtual Image Banner { get; set; }
	}
}
