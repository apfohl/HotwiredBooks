using Microsoft.AspNetCore.Mvc;

namespace HotwiredBooks.ViewComponents;

public sealed record HeaderData(int NumberOfBooks);

public sealed class HeaderViewComponent : ViewComponent
{
    public IViewComponentResult Invoke(HeaderData headerData) => View(headerData);
}
