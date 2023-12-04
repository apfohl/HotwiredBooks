using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HotwiredBooks.Pages;

public sealed class PrivacyModel(ILogger<PrivacyModel> logger) : PageModel
{
    private readonly ILogger<PrivacyModel> logger = logger;

    public void OnGet()
    {
    }
}
