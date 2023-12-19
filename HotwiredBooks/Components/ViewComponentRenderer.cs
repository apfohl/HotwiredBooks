using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace HotwiredBooks.Components;

public sealed class ViewComponentRenderer(ActionContext context, ITempDataProvider tempDataProvider)
{
    public async Task<IActionResult> RenderAsync(string viewComponentName, object viewModel)
    {
        var viewData = new ViewDataDictionary(new EmptyModelMetadataProvider(), new ModelStateDictionary())
        {
            Model = viewModel
        };

        await using var writer = new StringWriter();

        var viewContext = new ViewContext(
            context,
            new NullView(),
            viewData,
            new TempDataDictionary(context.HttpContext, tempDataProvider),
            writer,
            new HtmlHelperOptions()
        );

        var viewComponentHelper = context.HttpContext.RequestServices.GetRequiredService<IViewComponentHelper>();
        (viewComponentHelper as IViewContextAware)?.Contextualize(viewContext);

        var viewComponent = await viewComponentHelper.InvokeAsync(viewComponentName, viewModel);
        viewComponent.WriteTo(writer, HtmlEncoder.Default);

        await writer.FlushAsync();
        return new ContentResult
        {
            Content = writer.ToString(),
            ContentType = "text/html"
        };
    }

    private sealed class NullView : IView
    {
        public string Path => string.Empty;

        public Task RenderAsync(ViewContext context) => Task.CompletedTask;
    }
}
