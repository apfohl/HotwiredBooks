using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace HotwiredBooks.TagHelpers;

[HtmlTargetElement("turbo-frame")]
public sealed class TurboFrameTagHelper : TagHelper
{
    [HtmlAttributeName("id")]
    public string Id { get; set; }

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        var tagBuilder = new TagBuilder("turbo-frame");

        tagBuilder.Attributes.Add("id", Id);

        output.MergeAttributes(tagBuilder);
    }
}
