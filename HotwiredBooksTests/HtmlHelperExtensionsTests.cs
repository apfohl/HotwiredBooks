using HotwiredBooks.Extensions;

namespace HotwiredBooksTests;

[TestFixture]
public static class HtmlHelperExtensionsTests
{
    private sealed record BookWithId(Guid Id);

    private sealed record BookWithoutId;

    private static readonly Guid Id = Guid.NewGuid();

    private static IEnumerable<object[]> DomIdTestCases
    {
        get
        {
            yield return [typeof(BookWithId), null, "new_book_with_id"];
            yield return [new BookWithId(Id), null, $"book_with_id_{Id}"];
            yield return [new BookWithoutId(), null, "new_book_without_id"];
            yield return [typeof(BookWithId), "header", "header_book_with_id"];
            yield return [new BookWithId(Id), "header", $"header_book_with_id_{Id}"];
            yield return [new BookWithoutId(), "header", "header_book_without_id"];
        }
    }

    [TestCaseSource(nameof(DomIdTestCases))]
    public static void DomId_creates_correct_id(object objectOrType, string prefix, string expectedId)
    {
        var id = HtmlHelperExtensions.DomId(null, objectOrType, prefix);

        id.Should().Be(expectedId);
    }

    private static IEnumerable<object[]> DomClassTestCases
    {
        get
        {
            yield return [typeof(BookWithId), null, "book_with_id"];
            yield return [new BookWithId(Id), null, "book_with_id"];
            yield return [new BookWithoutId(), null, "book_without_id"];
            yield return [typeof(BookWithId), "header", "header_book_with_id"];
            yield return [new BookWithId(Id), "header", "header_book_with_id"];
            yield return [new BookWithoutId(), "header", "header_book_without_id"];
        }
    }

    [TestCaseSource(nameof(DomClassTestCases))]
    public static void DomClass_creates_correct_class(object objectOrType, string prefix, string expectedClass)
    {
        var @class = HtmlHelperExtensions.DomClass(null, objectOrType, prefix);

        @class.Should().Be(expectedClass);
    }
}
