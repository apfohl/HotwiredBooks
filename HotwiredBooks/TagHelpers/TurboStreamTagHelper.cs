using HotwiredBooks.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace HotwiredBooks.TagHelpers;

[HtmlTargetElement("turbo-stream")]
public sealed class TurboStreamTagHelper : TagHelper
{
    [HtmlAttributeName("action")] public TurboStreamAction Action { get; set; }

    [HtmlAttributeName("target")] public string Target { get; set; }

    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        var content = (await output.GetChildContentAsync()).GetContent();

        var templateTag = new TagBuilder("template");
        if (!string.IsNullOrEmpty(content) && !string.IsNullOrWhiteSpace(content))
        {
            templateTag.InnerHtml.AppendHtml(content);
        }
        else
        {
            templateTag.TagRenderMode = TagRenderMode.Normal;
        }

        output.Attributes.Add("action", Action.ToString().ToLower());
        output.Attributes.Add("target", Target);
        output.TagMode = TagMode.StartTagAndEndTag;

        output.Content.SetHtmlContent(templateTag);
    }
}
