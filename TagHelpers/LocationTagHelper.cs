using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cinematicks.TagHelpers
{
	public class LocationTagHelper : TagHelper
    {
		public string Address { get; set; }
		public string City { get; set; }

		public override void Process(TagHelperContext context, TagHelperOutput output)
		{
			output.TagName = "address";
			output.Content.SetHtmlContent(this.Address + @",<br />" + this.City);
		}
	}
}
