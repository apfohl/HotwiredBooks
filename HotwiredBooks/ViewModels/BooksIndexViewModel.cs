using HotwiredBooks.Models;

namespace HotwiredBooks.ViewModels;

public sealed record BooksIndexViewModel(IEnumerable<Book> Books)
{
    public int NumberOfBooks => Books.Count();
}
