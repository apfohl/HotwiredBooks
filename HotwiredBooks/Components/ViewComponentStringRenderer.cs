using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace HotwiredBooks.Components;

public sealed class ViewComponentStringRenderer(ActionContext controllerContext, ITempDataProvider tempDataProvider)
{
    public async Task<string> RenderAsync(string viewComponentName, object arguments)
    {
        var viewComponentHelper =
            controllerContext.HttpContext.RequestServices.GetRequiredService<IViewComponentHelper>();

        await using var writer = new StringWriter();

        var viewContext = new ViewContext(
            controllerContext,
            new NullView(),
            new ViewDataDictionary(new EmptyModelMetadataProvider(), new ModelStateDictionary()) { Model = arguments },
            new TempDataDictionary(controllerContext.HttpContext, tempDataProvider),
            writer,
            new HtmlHelperOptions()
        );

        (viewComponentHelper as IViewContextAware)?.Contextualize(viewContext);

        var renderedViewComponent = await viewComponentHelper.InvokeAsync(viewComponentName, arguments);

        renderedViewComponent.WriteTo(writer, HtmlEncoder.Default);

        await writer.FlushAsync();

        return writer.ToString();
    }

    private sealed class NullView : IView
    {
        public string Path => string.Empty;
        public Task RenderAsync(ViewContext context) => Task.CompletedTask;
    }
}
