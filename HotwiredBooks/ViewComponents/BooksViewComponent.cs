using HotwiredBooks.Components;
using HotwiredBooks.Models;
using HotwiredBooks.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace HotwiredBooks.ViewComponents;

public sealed class BooksViewComponent : ViewComponent
{
    public IViewComponentResult Invoke(IEnumerable<Book> books) => View(new BooksViewModel(books));
}
