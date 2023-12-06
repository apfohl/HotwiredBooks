using Microsoft.AspNetCore.Mvc.Filters;

namespace HotwiredBooks.Attributes;

public sealed class TurboStreamResponse : ActionFilterAttribute
{
    public override void OnResultExecuting(ResultExecutingContext context) =>
        context.HttpContext.Response.ContentType = "text/vnd.turbo-stream.html";
}
