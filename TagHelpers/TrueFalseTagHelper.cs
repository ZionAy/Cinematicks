using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cinematicks.TagHelpers
{
    public class TrueFalseTagHelper : TagHelper
	{
		public string State { get; set; }

		public override void Process(TagHelperContext context, TagHelperOutput output)
		{
			var image = (State == "True" ? "yes.png" : "no.png");
			output.TagName = "img";
			output.Attributes.SetAttribute("src", "../images/system/" + image);
		}
	}
}

