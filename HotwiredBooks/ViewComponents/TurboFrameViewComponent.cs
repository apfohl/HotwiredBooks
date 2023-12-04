using Microsoft.AspNetCore.Mvc;

namespace HotwiredBooks.ViewComponents;

public sealed class TurboFrameViewComponent : ViewComponent
{
    public Task<IViewComponentResult> InvokeAsync() =>
        Task.FromResult<IViewComponentResult>(View());
}
