using HotwiredBooks.Attributes;
using HotwiredBooks.Components;
using HotwiredBooks.Models;
using HotwiredBooks.ViewModels;
using Microsoft.AspNetCore.Mvc;
using MonadicBits;

namespace HotwiredBooks.Controllers;

using static Functional;

public sealed class BooksController(IBooksRepository booksRepository) : Controller
{
    private record FormData(string Title, string Author);

    [HttpGet]
    public async Task<ActionResult> Index() => View(new BooksIndexViewModel(
        (await booksRepository.All()).OrderBy(book => book.Title)));

    [HttpGet]
    public ActionResult New() => View();

    [HttpPost]
    [ValidateAntiForgeryToken]
    [TurboStreamResponse]
    public async Task<ActionResult> Create(IFormCollection collection)
    {
        var book = await (await ParseFormData(collection))
            .Match(
                formData => booksRepository.Create(formData.Title, formData.Author),
                () => Task.FromResult<Maybe<Book>>(Nothing)
            );

        return View(new BooksCreateViewModel(
            book.Match(b => b, () => throw new ArgumentException()),
            (await booksRepository.All()).Count())
        );
    }

    [HttpGet]
    public async Task<ActionResult> Edit(Guid id) =>
        (await booksRepository.Lookup(id))
        .Match(
            book => View(new BooksEditViewModel(book)),
            () => throw new ArgumentException()
        );

    [HttpPatch, HttpPut]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Update(Guid id, IFormCollection collection)
    {
        var saveBook =
            from formData in ParseFormData(collection)
            from book in booksRepository.Lookup(id)
            from updatedBook in booksRepository.Update(
                book with
                {
                    Title = formData.Title,
                    Author = formData.Author
                }
            )
            select updatedBook;

        var savedBook = await saveBook;

        return Created();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [TurboStreamResponse]
    public async Task<ActionResult> Delete(Guid id) =>
        await (await booksRepository
                .Lookup(id)
                .BindAsync(booksRepository.Delete))
            .Match(
                async book => View(
                    new BooksDeleteViewModel(
                        book,
                        (await booksRepository.All()).Count()
                    )
                ),
                () => throw new ArgumentException()
            );

    private static Task<Maybe<FormData>> ParseFormData(IFormCollection collection) =>
        Task.FromResult(
            from title in collection.TryGetValue("title", out var t) ? t.Just() : Nothing
            from author in collection.TryGetValue("author", out var a) ? a.Just() : Nothing
            select new FormData(title, author)
        );
}
