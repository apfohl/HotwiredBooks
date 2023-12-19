using HotwiredBooks.Models;
using HotwiredBooks.ViewModels;
using Microsoft.AspNetCore.Mvc;
using MonadicBits;

namespace HotwiredBooks.ViewComponents;

public sealed class BookFormViewComponent : ViewComponent
{
    public IViewComponentResult Invoke(Maybe<Book> book) => View(new BookFormViewModel(book));
}
