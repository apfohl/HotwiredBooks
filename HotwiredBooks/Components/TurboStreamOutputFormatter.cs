using System.Text;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;

namespace HotwiredBooks.Components;

public sealed class TurboStreamOutputFormatter : TextOutputFormatter
{
    public TurboStreamOutputFormatter()
    {
        SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse("text/vnd.turbo-stream.html"));
        SupportedEncodings.Add(Encoding.UTF8);
        SupportedEncodings.Add(Encoding.Unicode);
    }

    public override Task WriteResponseBodyAsync(OutputFormatterWriteContext context, Encoding selectedEncoding)
    {
        var response = context.HttpContext.Response;

        if (context.Object is string content)
        {
            return response.WriteAsync(content, selectedEncoding);
        }

        throw new NotSupportedException();
    }
}
