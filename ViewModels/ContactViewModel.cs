using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Cinematicks.ViewModels
{
    public class ContactViewModel
    {
		[Required(AllowEmptyStrings = false, ErrorMessage = "This field is required.")]
		[StringLength(50, MinimumLength = 2, ErrorMessage = "Your name must be 2-50 characters.")]
		[Display(Name = "Full Name")]
		public string Fullname { get; set; }

		[Required(AllowEmptyStrings = false, ErrorMessage = "This field is required.")]
		[DataType(DataType.PhoneNumber)]
		[Display(Name = "Phone")]
		public string Phone { get; set; }

		[Required(AllowEmptyStrings = false, ErrorMessage = "This field is required.")]
		[EmailAddress]
		[DataType(DataType.EmailAddress)]
		[Display(Name = "E-mail Address")]
		public string Email { get; set; }

		[Required(AllowEmptyStrings = false, ErrorMessage = "This field is required.")]
		[DataType(DataType.MultilineText)]
		[Display(Name = "Message")]
		public string Message { get; set; }
    }
}
