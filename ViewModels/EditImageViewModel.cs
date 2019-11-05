using Cinematicks.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Cinematicks.ViewModels
{
    public class EditImageViewModel
    {
		[Required]
		public int ID { get; set; }

		[Required(AllowEmptyStrings = false, ErrorMessage = "This field is required.")]
		[StringLength(80, MinimumLength = 2, ErrorMessage = "Name must be 2-80 characters.")]
		[Display(Name = "Image Name")]
		public string Name { get; set; }

		[Required(ErrorMessage = "This field is required.")]
		[Display(Name = "Image Category")]
		public ImageCategory Category { get; set; }

		[DataType(DataType.Upload)]
		[Display(Name = "Upload new Image")]
		public IFormFile Upload { get; set; }
	}
}
