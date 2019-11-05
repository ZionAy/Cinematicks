using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Cinematicks.Models
{
    public class Order
    {
		public int ID { get; set; }

		[Required(AllowEmptyStrings = false, ErrorMessage = "This field is required.")]
		[Display(Name = "Client")]
		public string ClientID { get; set; }

		[Column(TypeName = "smalldatetime")]
		[Display(Name = "Order Time")]
		public DateTime OrderTime { get; set; } = DateTime.Now;

		public Payment Payment { get; set; }


		public virtual Client Client { get; set; }
		public virtual ICollection<Ticket> Tickets { get; set; }
	}
}
