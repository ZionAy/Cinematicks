using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cinematicks.TagHelpers
{
    public class StarsTagHelper : TagHelper
    {
		public int Rating { get; set; }

		public override void Process(TagHelperContext context, TagHelperOutput output)
		{
			output.TagName = "div";
			var fullStars = "";
			var blankStars = "";
			for (var i = 1; i <= Rating; i++)
			{
				fullStars += "<i class=\"fa fa-star\"></i>";
			}
			for (var i = 1; i <= 10 - Rating; i++)
			{
				blankStars += "<i class=\"fa fa-star\"></i>";
			}
			output.Content.SetHtmlContent("<span class=\"full-stars\">" + fullStars + "</span><span class=\"empty-stars\">" + blankStars + "</span>");
		}
	}
}
