using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Cinematicks.Models
{
    public class Cinema
    {
		public int ID { get; set; }

		[Required(AllowEmptyStrings = false, ErrorMessage = "This field is required.")]
		[StringLength(30, MinimumLength = 2, ErrorMessage = "Cinema name must be 2-30 characters.")]
		[Display(Name = "Name")]
		public string Name { get; set; }

		[Required(AllowEmptyStrings = false, ErrorMessage = "This field is required.")]
		[StringLength(2000, MinimumLength = 5, ErrorMessage = "Cinema about must be 5-2000 characters.")]
		[DataType(DataType.MultilineText)]
		[Display(Name = "About")]
		public string About { get; set; }

		[Required(ErrorMessage = "This field is required.")]
		[Range(0.99, 500, ErrorMessage = "Ticket price must be between 0.99 to 500.")]
		[Display(Name = "Ticket price")]
		public float Price { get; set; }

		[Required(ErrorMessage = "This field is required.")]
		[Display(Name = "Cinema Address")]
		public int LocationID { get; set; }

		[Required(ErrorMessage = "This field is required.")]
		[Display(Name = "Cinema Image")]
		public int PhotoID { get; set; }


		public virtual Location Location { get; set; }
		public virtual Image Photo { get; set; }
		public virtual ICollection<Hall> Halls { get; set; }
		public virtual ICollection<PhotoGal> Gallery { get; set; }
	}
}
