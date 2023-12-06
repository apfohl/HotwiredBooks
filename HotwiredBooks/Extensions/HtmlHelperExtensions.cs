using System.Reflection;
using Humanizer;
using Microsoft.AspNetCore.Mvc.Rendering;
using MonadicBits;

namespace HotwiredBooks.Extensions;

public static class HtmlHelperExtensions
{
    private const string Join = "_";
    private const string New = "new";

    public static string DomId(this IHtmlHelper htmlHelper, object @object, string prefix = null) =>
    (
        from property in @object.GetType().GetProperty("Id").ToMaybe()
        from value in property.GetValue(@object).ToMaybe()
        select value.ToString()
    ).Match(
        id => $"{DomClass(htmlHelper, @object, prefix)}{Join}{id}",
        () => DomClass(htmlHelper, @object, prefix ?? New)
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
