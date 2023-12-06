using HotwiredBooks.Models;
using HotwiredBooks.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace HotwiredBooks.ViewComponents;

public sealed class BookViewComponent : ViewComponent
{
    public IViewComponentResult Invoke(Book book) => View(new BookViewModel(book));
}
