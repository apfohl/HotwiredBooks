namespace HotwiredBooks.Models;

public sealed record Book(Guid Id, string Title, string Author, DateTime CreatedAt);
