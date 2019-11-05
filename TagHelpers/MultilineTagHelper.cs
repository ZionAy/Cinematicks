using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cinematicks.TagHelpers
{
	public class MultilineTagHelper : TagHelper
    {
		public string Text { get; set; }

		public override void Process(TagHelperContext context, TagHelperOutput output)
		{
			output.TagName = "p";
			output.Content.SetHtmlContent(Text.Replace(Environment.NewLine, "<br />"));
		}
	}
}
