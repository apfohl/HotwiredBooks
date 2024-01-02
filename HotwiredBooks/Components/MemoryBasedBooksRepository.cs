using System.Collections.Concurrent;
using bridgefield.FoundationalBits;
using HotwiredBooks.Models;
using ErrorOr;
using ErrorOr.Extensions;
using HotwiredBooks.Extensions;

namespace HotwiredBooks.Components;

internal interface IBooksCommand;

internal sealed record Create(string Title, string Author, DateTime CreatedAt) : IBooksCommand;

internal sealed record Update(Book Book) : IBooksCommand;

internal sealed record Delete(Book Book) : IBooksCommand;

public sealed class MemoryBasedBooksRepository : IBooksRepository
{
    private readonly IAgent<IBooksCommand, ErrorOr<Book>> agent;
    private readonly ConcurrentDictionary<Guid, Book> books = new(InitialBooks());

    public MemoryBasedBooksRepository() =>
        agent = Agent.Start<ConcurrentDictionary<Guid, Book>, IBooksCommand, ErrorOr<Book>>(
            books,
            (current, command) => command switch
            {
                Create create => (current,
                    from book in new Book(Guid.NewGuid(), create.Title, create.Author, create.CreatedAt).Success()
                    from createdBook in current.TryAdd(book.Id, book) ? book.Success() : ErrorOr<Book>.From([Error.Failure()])
                    select createdBook).AsTask(),
                Delete delete => (current,
                    current.TryRemove(delete.Book.Id, out var deletedBook) ? deletedBook.Success() : ErrorOr<Book>.From(
                        [Error.Failure()])).AsTask(),
                Update update => Task.FromResult((current,
                    from currentBook in current.TryGetValue(update.Book.Id, out var currentValue)
                        ? currentValue.Success()
                        : ErrorOr<Book>.From([Error.Failure()])
                    from updatedBook in current.TryUpdate(update.Book.Id, update.Book, currentBook)
                        ? update.Book.Success()
                        : ErrorOr<Book>.From([Error.Failure()])
                    select updatedBook)),
                _ => throw new ArgumentOutOfRangeException(nameof(command))
            });

    public Task<ErrorOr<Book>> Lookup(Guid id) =>
        Task.FromResult(books.TryGetValue(id, out var book) ? ErrorOrFactory.From(book) : Error.NotFound());

    public Task<IEnumerable<Book>> All() =>
        Task.FromResult<IEnumerable<Book>>(books.Values);

    public Task<ErrorOr<Book>> Create(string title, string author) =>
        agent.Tell(new Create(title, author, DateTime.Now));

    public Task<ErrorOr<Book>> Update(Book book) =>
        agent.Tell(new Update(book));

    public Task<ErrorOr<Book>> Delete(Book book) =>
        agent.Tell(new Delete(book));

    private static IEnumerable<KeyValuePair<Guid, Book>> InitialBooks()
    {
        yield return CreateBook("The Art of Computer Programming", "Donald E. Knuth");
        yield return CreateBook("Domain Modelling Made Functional", "Scott Wlaschin");
        yield return CreateBook("The Pragmatic Programmer", "Andrew Hunt and David Thomas");
        yield return CreateBook("Domain Driven Design Distilled", "Vaughn Vernon");
        yield return CreateBook("Clean Architecture", "Robert C. Martin");
        yield return CreateBook("Domain Driven Design", "Eric Evans");
        yield return CreateBook("Refactoring", "Martin Fowler");
        yield return CreateBook("Implementing Domain Driven Design", "Vaughn Vernon");
    }

    private static KeyValuePair<Guid, Book> CreateBook(string title, string author)
    {
        var book = new Book(Guid.NewGuid(), title, author, DateTime.Now);

        return KeyValuePair.Create(book.Id, book);
    }
}
