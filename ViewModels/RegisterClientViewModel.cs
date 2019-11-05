using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Cinematicks.ViewModels
{
    public class RegisterClientViewModel
    {
		[Required(AllowEmptyStrings = false, ErrorMessage = "This field is required.")]
		[StringLength(30, MinimumLength = 2, ErrorMessage = "Name must be 2-30 characters.")]
		[Display(Name = "Username")]
		public string UserName { get; set; }

		[Required(AllowEmptyStrings = false, ErrorMessage = "This field is required.")]
		[EmailAddress]
		[DataType(DataType.EmailAddress)]
		[Display(Name = "Email")]
		public string Email { get; set; }

		[Required(AllowEmptyStrings = false, ErrorMessage = "This field is required.")]
		[StringLength(16, MinimumLength = 6, ErrorMessage = "Password must be 6-16 characters.")]
		[DataType(DataType.Password)]
		[Display(Name = "Password")]
		public string Password { get; set; }

		[DataType(DataType.Password)]
		[Display(Name = "Confirm password")]
		[Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
		public string ConfirmPassword { get; set; }

		[Required(ErrorMessage = "This field is required.")]
		[Display(Name = "Avatar")]
		public int AvatarID { get; set; }
	}
}
