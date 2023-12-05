using HotwiredBooks.Components;
using HotwiredBooks.Extensions;
using HotwiredBooks.Models;
using HotwiredBooks.ViewComponents;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using MonadicBits;

namespace HotwiredBooks.Controllers;

using static Functional;

public sealed class BooksController(IBooksRepository booksRepository, ITempDataProvider tempDataProvider)
    : Controller
{
    // GET: BooksController
    public async Task<ActionResult> Index() => View(
        new BooksViewModel((await booksRepository.All()).OrderBy(book => book.Title))
    );

    // GET: BooksController/Create
    public ActionResult Create() => View();

    // POST: BooksController/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Create(IFormCollection collection)
    {
        var formData =
            from title in collection.TryGetValue("title", out var t) ? t.Just() : Nothing
            from author in collection.TryGetValue("author", out var a) ? a.Just() : Nothing
            select (Title: title, Author: author);

        await formData.Match(
            fd => booksRepository.Create(fd.Title, fd.Author),
            () => Task.CompletedTask
        );

        HttpContext.SetTurboStreamMimeType();

        var htmlElements = new[]
        {
            await TurboStream("books_heading", TurboStreamAction.Update,
                (await Header((await booksRepository.All()).Count())).Just()),
            await TurboStream("new_book", TurboStreamAction.Update, string.Empty.Just()),
            await TurboStream("new_book", TurboStreamAction.After,
                (await Books((await booksRepository.All()))).Just())
        };

        return Ok(string.Concat(htmlElements));
    }

    // GET: BooksController/Edit/5
    public ActionResult Edit(int id) =>
        throw new NotImplementedException();

    // POST: BooksController/Edit/5
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

    // POST: BooksController/Delete/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Delete(int id, IFormCollection collection)
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

    private Task<string> TurboStream(string target, TurboStreamAction action, Maybe<string> template)
    {
        var renderer = new ViewComponentStringRenderer(ControllerContext, tempDataProvider);
        return renderer.RenderAsync("TurboStream", new { target, action, template });
    }

    private Task<string> Header(int numberOfBooks)
    {
        var renderer = new ViewComponentStringRenderer(ControllerContext, tempDataProvider);
        return renderer.RenderAsync("Header", new { headerData = new HeaderData(numberOfBooks) });
    }

    private Task<string> Books(IEnumerable<Book> books)
    {
        var renderer = new ViewComponentStringRenderer(ControllerContext, tempDataProvider);
        return renderer.RenderAsync("Books", new { booksData = new BooksData(books) });
    }
}
