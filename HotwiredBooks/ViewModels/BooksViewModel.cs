using HotwiredBooks.Models;

namespace HotwiredBooks.ViewModels;

public sealed class BooksViewModel(IEnumerable<Book> books)
{
    public IEnumerable<Book> Books => books;
}
