using HotwiredBooks.Models;
using MonadicBits;

namespace HotwiredBooks.Components;

public interface IBooksRepository
{
    Task<Maybe<Book>> Lookup(Guid id);
    Task<IEnumerable<Book>> All();
    Task<Maybe<Book>> Create(string title, string author);
    Task<Maybe<Book>> Update(Book book);
    Task<Maybe<Book>> Delete(Book book);
}
