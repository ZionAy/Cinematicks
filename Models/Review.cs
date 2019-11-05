using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Cinematicks.Models
{
    public class Review
    {
		public int ID { get; set; }

		[Required(ErrorMessage = "This field is required.")]
		[Range(1, 10, ErrorMessage = "Rating must be 1-10 stars.")]
		[Display(Name = "Rating")]
		public int Rating { get; set; }

		[Required(AllowEmptyStrings = false, ErrorMessage = "This field is required.")]
		[StringLength(1000, MinimumLength = 3, ErrorMessage = "Review must be 3-1000 characters.")]
		[Display(Name = "Review")]
		public string Comment { get; set; }

		[Required(ErrorMessage = "This field is required.")]
		[Display(Name = "Movie")]
		public int MovieID { get; set; }

		[Required(ErrorMessage = "This field is required.")]
		[Display(Name = "Client")]
		public string ClientID { get; set; }

		[Column(TypeName = "smalldatetime")]
		[Display(Name = "Posted on")]
		public DateTime PostTime { get; set; } = DateTime.Now;


		public virtual Movie Movie { get; set; }
		public virtual Client Client { get; set; }
	}
}
