using System.Collections.Concurrent;
using bridgefield.FoundationalBits;
using HotwiredBooks.Models;
using MonadicBits;

namespace HotwiredBooks.Components;

using static Functional;

internal interface IBooksCommand;

internal sealed record Create(string Title, string Author) : IBooksCommand;

internal sealed record Update(Book Book) : IBooksCommand;

internal sealed record Delete(Book Book) : IBooksCommand;

public sealed class MemoryBasedBooksRepository : IBooksRepository
{
    private readonly IAgent<IBooksCommand, Maybe<Book>> agent;
    private readonly ConcurrentDictionary<Guid, Book> books = new(InitialBooks());

    public MemoryBasedBooksRepository() =>
        agent = Agent.Start<ConcurrentDictionary<Guid, Book>, IBooksCommand, Maybe<Book>>(
            books,
            (current, command) => command switch
            {
                Create create => Task.FromResult((current,
                    from book in new Book(Guid.NewGuid(), create.Title, create.Author).Just()
                    from createdBook in current.TryAdd(book.Id, book) ? book.Just() : Nothing
                    select createdBook)),
                Delete delete => Task.FromResult((current,
                    current.TryRemove(delete.Book.Id, out var deletedBook) ? deletedBook.Just() : Nothing)),
                Update update => Task.FromResult((current,
                    from currentBook in current.TryGetValue(update.Book.Id, out var currentValue)
                        ? currentValue.Just()
                        : Nothing
                    from updatedBook in current.TryUpdate(update.Book.Id, update.Book, currentBook)
                        ? update.Book.Just()
                        : Nothing
                    select updatedBook)),
                _ => throw new ArgumentOutOfRangeException(nameof(command))
            });

    public Task<Maybe<Book>> Lookup(Guid id) =>
        Task.FromResult(books.TryGetValue(id, out var book) ? book.Just() : Nothing);

    public Task<IEnumerable<Book>> All() =>
        Task.FromResult<IEnumerable<Book>>(books.Values);

    public Task<Maybe<Book>> Create(string title, string author) =>
        agent.Tell(new Create(title, author));

    public Task<Maybe<Book>> Update(Book book) =>
        agent.Tell(new Update(book));

    public Task<Maybe<Book>> Delete(Book book) =>
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
        var book = new Book(Guid.NewGuid(), title, author);

        return KeyValuePair.Create(book.Id, book);
    }
}
