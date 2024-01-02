using HotwiredBooks.Models;
using ErrorOr;

namespace HotwiredBooks.Components;

public interface IBooksRepository
{
    Task<ErrorOr<Book>> Lookup(Guid id);
    Task<IEnumerable<Book>> All();
    Task<ErrorOr<Book>> Create(string title, string author);
    Task<ErrorOr<Book>> Update(Book book);
    Task<ErrorOr<Book>> Delete(Book book);
}
