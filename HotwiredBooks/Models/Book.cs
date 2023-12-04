namespace HotwiredBooks.Models;

public sealed record Book(Guid Id, string Title, string Author)
{
    public Book Create(Guid id, string title, string author) =>
        new(id, title, author);
}
