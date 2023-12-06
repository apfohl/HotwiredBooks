using System.Reactive;
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
        var formData =
            from title in collection.TryGetValue("title", out var t) ? t.Just() : Nothing
            from author in collection.TryGetValue("author", out var a) ? a.Just() : Nothing
            select (Title: title, Author: author);

        var book = await formData.Match(
            fd => booksRepository.Create(fd.Title, fd.Author),
            () => Task.FromResult<Maybe<Book>>(Nothing)
        );

        return View(new BooksCreateViewModel(
            book.Match(b => b, () => throw new ArgumentException()),
            (await booksRepository.All()).Count())
        );
    }

    [HttpGet]
    public ActionResult Edit(int id) =>
        throw new NotImplementedException();

    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Edit(int id, IFormCollection collection)
    {
        try
        {
            return RedirectToAction(nameof(Index));
        }
        catch
        {
            throw new NotImplementedException();
        }
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
}
