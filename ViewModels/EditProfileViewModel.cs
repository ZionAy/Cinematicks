using Cinematicks.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Cinematicks.ViewModels
{
    public class EditProfileViewModel
    {
		[Required(AllowEmptyStrings = false, ErrorMessage = "This field is required.")]
		[Display(Name = "Username")]
		public string Username { get; set; }

		[Required(AllowEmptyStrings = false, ErrorMessage = "This field is required.")]
		[EmailAddress]
		[DataType(DataType.EmailAddress)]
		[Display(Name = "Email")]
		public string Email { get; set; }

		[Required(ErrorMessage = "This field is required.")]
		[Display(Name = "Avatar")]
		public int AvatarID { get; set; }

		[NotMapped]
		public Image Avatar { get; set; }
	}
}
