using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Cinematicks.Models
{
    public class Client : IdentityUser
	{
		[Required(ErrorMessage = "This field is required.")]
		[Display(Name = "Avatar")]
		public int AvatarID { get; set; }

		[Column(TypeName = "smalldatetime")]
		[Display(Name = "Registered on")]
		public DateTime RegisterTime { get; set; } = DateTime.Now;
		

		public Image Avatar { get; set; }
		public virtual ICollection<Review> Reviews { get; set; }
		public virtual ICollection<Ticket> Tickets { get; set; }
		public virtual ICollection<Order> Orders { get; set; }
	}
}
