using HotwiredBooks.Models;

namespace HotwiredBooks.ViewModels;

public sealed class BookViewModel(Book book)
{
    public Guid Id => book.Id;

    public string Title => book.Title;

    public string Author => book.Author;
}
