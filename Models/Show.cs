using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Cinematicks.Models
{
    public class Show
    {
		public int ID { get; set; }

		[Required(ErrorMessage = "This field is required.")]
		[Display(Name = "Hall")]
		public int HallID { get; set; }

		[Required(ErrorMessage = "This field is required.")]
		[Display(Name = "Movie")]
		public int MovieID { get; set; }

		[Column(TypeName = "date")]
		[Required(ErrorMessage = "This field is required.")]
		[DataType(DataType.Date)]
		[Display(Name = "Show Date")]
		public DateTime ShowDate { get; set; }

		[Column(TypeName = "time")]
		[Required(ErrorMessage = "This field is required.")]
		[DataType(DataType.Time)]
		[Display(Name = "Show Time")]
		public TimeSpan ShowTime { get; set; }

		[NotMapped]
		public string TimeShow
		{
			get
			{
				return this.ShowDate.ToString("dd/MM/yyyy") + " " + this.ShowTime.ToString(@"hh\:mm");
			}
		}

		public virtual Hall Hall { get; set; }
		public virtual Movie Movie { get; set; }
		public virtual ICollection<Ticket> Tickets { get; set; }
	}
}
