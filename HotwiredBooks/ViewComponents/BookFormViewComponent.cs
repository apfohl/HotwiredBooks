using HotwiredBooks.Models;
using HotwiredBooks.ViewModels;
using Microsoft.AspNetCore.Mvc;
using ErrorOr;

namespace HotwiredBooks.ViewComponents;

public sealed class BookFormViewComponent : ViewComponent
{
    public IViewComponentResult Invoke(ErrorOr<Book> book) => View(new BookFormViewModel(book));
}
