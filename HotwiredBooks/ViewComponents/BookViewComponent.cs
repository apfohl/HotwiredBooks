using HotwiredBooks.Models;
using Microsoft.AspNetCore.Mvc;

namespace HotwiredBooks.ViewComponents;

public sealed record BookData(Book Book);

public sealed class BookViewComponent : ViewComponent
{
    public IViewComponentResult Invoke(BookData bookData) => View(bookData);
}
