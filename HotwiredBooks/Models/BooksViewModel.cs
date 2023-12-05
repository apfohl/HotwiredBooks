using HotwiredBooks.ViewComponents;

namespace HotwiredBooks.Models;

public sealed class BooksViewModel(IEnumerable<Book> books)
{
    public BooksData BooksData => new(books);
    public HeaderData HeaderData => new(books.Count());
}
