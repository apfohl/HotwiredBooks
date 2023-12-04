using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HotwiredBooks.Pages;

public sealed class IndexModel(ILogger<IndexModel> logger) : PageModel
{
    private readonly ILogger<IndexModel> logger = logger;

    public void OnGet()
    {
    }
}
