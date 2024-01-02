using System.Reflection;
using ErrorOr.Extensions;
using Humanizer;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HotwiredBooks.Extensions;

public static class HtmlHelperExtensions
{
    private const string Join = "_";
    private const string New = "new";

    public static string DomId(this IHtmlHelper htmlHelper, object @object, string prefix = null) =>
    (
        from property in @object.GetType().GetProperty("Id").ToErrorOr()
        from value in property.GetValue(@object).ToErrorOr()
        select value.ToString()
    ).Match(
        id => $"{DomClass(htmlHelper, @object, prefix)}{Join}{id}",
        _ => DomClass(htmlHelper, @object, prefix ?? New)
    );

    public static string DomClass(this IHtmlHelper _, object objectOrType, string prefix = null)
    {
        var id = objectOrType switch
        {
            Type type => HandleType(type),
            not null => HandleObject(objectOrType)
        };

        return prefix is not null ? $"{prefix}{Join}{id}" : id;
    }

    private static string HandleType(MemberInfo type) =>
        type.Name.Humanize(LetterCasing.LowerCase).Replace(" ", "_");

    private static string HandleObject(object @object) =>
        HandleType(@object.GetType());
}
