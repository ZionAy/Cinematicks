using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Cinematicks.Models
{
    public class Movie
    {
		public int ID { get; set; }

		[Required(AllowEmptyStrings = false, ErrorMessage = "This field is required.")]
		[StringLength(100, MinimumLength = 1, ErrorMessage = "Movie title must be 1-100 characters.")]
		[Display(Name = "Title")]
		public string Title { get; set; }

		[Column(TypeName = "date")]
		[Required(ErrorMessage = "This field is required.")]
		[DataType(DataType.Date)]
		[Display(Name = "Release Date")]
		public DateTime Release { get; set; }

		[Required(ErrorMessage = "This field is required.")]
		[Range(5, 240, ErrorMessage = "Movie duration must be 5-240 minutes.")]
		[Display(Name = "Duration")]
		public int Time { get; set; }

		[Required(ErrorMessage = "This field is required.")]
		[Display(Name = "Movie Rate")]
		public int RateID { get; set; }

		[Required(ErrorMessage = "This field is required.")]
		[Display(Name = "Dubbed")]
		public bool IsDub { get; set; } = false;

		[Required(AllowEmptyStrings = false, ErrorMessage = "This field is required.")]
		[StringLength(60, MinimumLength = 3, ErrorMessage = "Director must be 3-60 characters.")]
		[Display(Name = "Directed By")]
		public string Director { get; set; }

		[Required(AllowEmptyStrings = false, ErrorMessage = "This field is required.")]
		[StringLength(100, MinimumLength = 3, ErrorMessage = "Starring must be 3-100 characters.")]
		[Display(Name = "Starring")]
		public string Actors { get; set; }

		[Required(AllowEmptyStrings = false, ErrorMessage = "This field is required.")]
		[StringLength(1500, MinimumLength = 3, ErrorMessage = "Plot must be 3-1500 characters.")]
		[DataType(DataType.MultilineText)]
		[Display(Name = "Plot")]
		public string Plot { get; set; }

		[Required(AllowEmptyStrings = false, ErrorMessage = "This field is required.")]
		[StringLength(11, MinimumLength = 11, ErrorMessage = "Trailer must be exactly 11 characters.")]
		[Display(Name = "YouTube Trailer")]
		public string Trailer { get; set; }

		[Required(ErrorMessage = "This field is required.")]
		[Display(Name = "Poster Image")]
		public int PosterID { get; set; }


		public virtual Rate Rate { get; set; }
		public virtual Image Poster { get; set; }
		public virtual ICollection<MovGen> Genres { get; set; }
		public virtual ICollection<Review> Reviews { get; set; }
		public virtual ICollection<Show> Shows { get; set; }
	}
}
