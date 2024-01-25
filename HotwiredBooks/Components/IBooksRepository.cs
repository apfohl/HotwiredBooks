using AwesomeResult;
using HotwiredBooks.Models;

namespace HotwiredBooks.Components;

public interface IBooksRepository
{
    Task<Result<Book>> Lookup(Guid id);
    Task<IEnumerable<Book>> All();
    Task<Result<Book>> Create(string title, string author);
    Task<Result<Book>> Update(Book book);
    Task<Result<Book>> Delete(Book book);
}
