using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Cinematicks.Models
{
    public class Payment
    {
		public int ID { get; set; }

		[Required(AllowEmptyStrings = false, ErrorMessage = "This field is required.")]
		[StringLength(30, MinimumLength = 2, ErrorMessage = "First name must be 2-30 characters.")]
		[Display(Name = "First Name")]
		public string FirstName { get; set; }

		[Required(AllowEmptyStrings = false, ErrorMessage = "This field is required.")]
		[StringLength(30, MinimumLength = 2, ErrorMessage = "Last name must be 2-30 characters.")]
		[Display(Name = "Last Name")]
		public string LastName { get; set; }

		[Required(AllowEmptyStrings = false, ErrorMessage = "This field is required.")]
		[StringLength(9, MinimumLength = 9, ErrorMessage = "Card Holder ID must be 9 characters.")]
		[Display(Name = "Card Holder ID")]
		public string CCID { get; set; }

		[Required(AllowEmptyStrings = false, ErrorMessage = "This field is required.")]
		[StringLength(16, MinimumLength = 8, ErrorMessage = "Credit card number invalid.")]
		[DataType(DataType.CreditCard)]
		[Display(Name = "Card Number")]
		public string CCNum { get; set; }

		[Required(AllowEmptyStrings = false, ErrorMessage = "This field is required.")]
		[StringLength(3, MinimumLength = 3, ErrorMessage = "CVV must be 3 characters.")]
		[Display(Name = "CVV")]
		public string CCCVV { get; set; }

		[Required(ErrorMessage = "This field is required.")]
		[Range(1, 12, ErrorMessage = "Month must be in 1-12 range.")]
		[Display(Name = "Expire Month")]
		public int CCMonth { get; set; }

		[Required(ErrorMessage = "This field is required.")]
		[Range(2018, 2028, ErrorMessage = "Year must be in 2018-2028 range.")]
		[Display(Name = "Expire Year")]
		public int CCYear { get; set; }

		[Required(ErrorMessage = "This field is required.")]
		[Display(Name = "Send Me Invoice")]
		public bool SendInvoice { get; set; } = false;

		[StringLength(80, MinimumLength = 2, ErrorMessage = "Address must be 2-80 characters.")]
		[Display(Name = "Address")]
		public string Address { get; set; }

		[StringLength(30, MinimumLength = 2, ErrorMessage = "Cityt must be 2-30 characters.")]
		[Display(Name = "City")]
		public string City { get; set; }

		[StringLength(7, MinimumLength = 5, ErrorMessage = "Zip code must be 5-7 characters.")]
		[DataType(DataType.PostalCode)]
		[Display(Name = "Zip Code")]
		public string ZipCode { get; set; }

		[Required(ErrorMessage = "This field is required.")]
		[Display(Name = "For Order")]
		public int OrderID { get; set; }


		public virtual Order Order { get; set; }
	}
}
