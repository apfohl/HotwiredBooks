using HotwiredBooks.Components;
using AwesomeResult;

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
        book.Switch(b =>
            {
                b.Title.Should().Be(title);
                b.Author.Should().Be(author);
                b.Id.Should().NotBe(Guid.Empty);
            },
            _ => Assert.Fail()
        );

        var areEqual =
            from created in Task.FromResult(book)
            from read in repository.Lookup(created.Id)
            select created == read;

        (await areEqual).Switch(eq => eq.Should().BeTrue(), _ => Assert.Fail());
    }
}
