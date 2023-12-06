using HotwiredBooks.Models;
using HotwiredBooks.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace HotwiredBooks.ViewComponents;

public sealed class HeaderViewComponent : ViewComponent
{
    public IViewComponentResult Invoke(int numberOfBooks) => View(new HeaderViewModel(numberOfBooks));
}
