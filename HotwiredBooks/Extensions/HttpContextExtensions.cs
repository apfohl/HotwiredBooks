namespace HotwiredBooks.Extensions;

public static class HttpContextExtensions
{
    public static void SetTurboStreamMimeType(this HttpContext httpContext) =>
        httpContext.Response.ContentType = "text/vnd.turbo-stream.html";
}
