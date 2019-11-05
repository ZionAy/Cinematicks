using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Cinematicks.Models
{
    public class Location
    {
		public int ID { get; set; }

		[Required(AllowEmptyStrings = false, ErrorMessage = "This field is required.")]
		[StringLength(30, MinimumLength = 2, ErrorMessage = "City must be 2-30 characters.")]
		[Display(Name = "City")]
		public string City { get; set; }

		[Required(AllowEmptyStrings = false, ErrorMessage = "This field is required.")]
		[StringLength(80, MinimumLength = 3, ErrorMessage = "Address must be 3-80 characters.")]
		[Display(Name = "Address")]
		public string Address { get; set; }

		[Required(AllowEmptyStrings = false, ErrorMessage = "This field is required.")]
		[StringLength(1000, MinimumLength = 2, ErrorMessage = "Directions must be 2-1000 characters.")]
		[DataType(DataType.MultilineText)]
		[Display(Name = "Directions")]
		public string Directions { get; set; }

		[Required(ErrorMessage = "This field is required.")]
		[Display(Name = "Map Image")]
		public int MapID { get; set; }

		[NotMapped]
		public string FullAddress
		{
			get { return this.Address + ", " + this.City; }
		}


		public virtual Image Map { get; set; }
		public virtual ICollection<Cinema> Cinemas { get; set; }
	}
}
