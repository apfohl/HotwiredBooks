using Microsoft.AspNetCore.Mvc;

namespace HotwiredBooks.ViewComponents;

public sealed class TurboFrameViewComponent : ViewComponent
{
    public IViewComponentResult Invoke() =>
        View();
}
