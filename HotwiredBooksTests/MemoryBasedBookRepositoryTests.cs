using ErrorOr.Extensions;
using HotwiredBooks.Components;

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

        var book = await repository.Create(title, author);
        book.Switch(b => Assert.Multiple(() =>
            {
                Assert.That(b.Title, Is.EqualTo(title));
                Assert.That(b.Author, Is.EqualTo(author));
                Assert.That(b.Id, Is.Not.EqualTo(new Guid()));
            }),
            _ => Assert.Fail()
        );

        var areEqual =
            from created in Task.FromResult(book)
            from read in repository.Lookup(created.Id)
            select created == read;

        (await areEqual).Switch(_ => Assert.Pass(), _ => Assert.Fail());
    }
}
