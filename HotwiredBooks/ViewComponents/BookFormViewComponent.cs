using AwesomeResult;
using HotwiredBooks.Models;
using HotwiredBooks.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace HotwiredBooks.ViewComponents;

public sealed class BookFormViewComponent : ViewComponent
{
    public IViewComponentResult Invoke(Result<Book> book) => View(new BookFormViewModel(book));
}
