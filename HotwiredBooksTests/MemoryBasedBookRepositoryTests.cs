using HotwiredBooks.Components;
using MonadicBits;

namespace HotwiredBooksTests;

[TestFixture]
public static class MemoryBasedBookRepositoryTests
{
    [Test]
    public static async Task Create_inserts_value_into_repository()
    {
        var repository = new MemoryBasedBooksRepository();
        const string title = "ABC";
        const string author = "DEF";

        var maybe = await repository.Create(title, author);
        maybe.Match(book =>
            {
                Assert.That(book.Title, Is.EqualTo(title));
                Assert.That(book.Author, Is.EqualTo(author));
                Assert.That(book.Id, Is.Not.EqualTo(new Guid()));
            },
            Assert.Fail
        );

        var areEqual =
            from created in Task.FromResult(maybe)
            from read in repository.Lookup(created.Id)
            select created == read;

        (await areEqual).Match(Assert.True, Assert.Fail);
    }
}
