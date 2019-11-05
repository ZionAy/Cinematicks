using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Cinematicks.Models
{
    public class Ticket
    {
		public int ID { get; set; }

		[Required(ErrorMessage = "This field is required.")]
		[Display(Name = "Show")]
		public int ShowID { get; set; }

		[Required(ErrorMessage = "This field is required.")]
		[Display(Name = "Row Number")]
		public int Row { get; set; }

		[Required(ErrorMessage = "This field is required.")]
		[Display(Name = "Seat Number")]
		public int Col { get; set; }

		[Required(ErrorMessage = "This field is required.")]
		[Display(Name = "Client")]
		public string ClientID { get; set; }

		[Display(Name = "Order")]
		public int OrderID { get; set; }


		public virtual Show Show { get; set; }
		public virtual Client Client { get; set; }
		public virtual Order Order { get; set; }		
	}
}
