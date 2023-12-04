using HotwiredBooks.Models;
using Microsoft.AspNetCore.Mvc;

namespace HotwiredBooks.ViewComponents;

public sealed class BookViewComponent : ViewComponent
{
    public IViewComponentResult Invoke(Book book) =>
        View(book);
}
