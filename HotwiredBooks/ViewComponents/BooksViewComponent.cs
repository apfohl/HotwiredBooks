using HotwiredBooks.Components;
using HotwiredBooks.Models;
using Microsoft.AspNetCore.Mvc;

namespace HotwiredBooks.ViewComponents;

public sealed record BooksData(IEnumerable<Book> Books);

public sealed class BooksViewComponent : ViewComponent
{
    public IViewComponentResult Invoke(BooksData booksData) => View(booksData);
}
