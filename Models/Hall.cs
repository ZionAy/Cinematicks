using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Cinematicks.Models
{
    public class Hall
    {
		public int ID { get; set; }

		[Required(AllowEmptyStrings = false, ErrorMessage = "This field is required.")]
		[StringLength(30, MinimumLength = 1, ErrorMessage = "Hall name must be 1-30 characters.")]
		[Display(Name = "Name")]
		public string Name { get; set; }

		[Required(ErrorMessage = "This field is required.")]
		[Range(5, 30, ErrorMessage = "Hall rows must be 5-30 rows only.")]
		[Display(Name = "Hall Rows")]
		public int Rows { get; set; }

		[Required(ErrorMessage = "This field is required.")]
		[Range(5, 30, ErrorMessage = "Row seats must be 5-30 rows only.")]
		[Display(Name = "Row Seats")]
		public int Cols { get; set; }

		[Required(ErrorMessage = "This field is required.")]
		[Display(Name = "In Cinema")]
		public int CinemaID { get; set; }


		public virtual Cinema Cinema { get; set; }
		public virtual ICollection<Show> Shows { get; set; }
	}
}
