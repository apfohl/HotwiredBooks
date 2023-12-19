using HotwiredBooks.Attributes;
using HotwiredBooks.Components;
using HotwiredBooks.Extensions;
using HotwiredBooks.Models;
using HotwiredBooks.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using MonadicBits;

namespace HotwiredBooks.Controllers;

public sealed class BooksController(IBooksRepository booksRepository, ITempDataProvider tempDataProvider)
    : Controller
{
    private record FormData(string Title, string Author);

    private ViewComponentRenderer ViewComponentRenderer => new(ControllerContext, tempDataProvider);

    [HttpGet]
    public async Task<IActionResult> Index() => View(new BooksIndexViewModel(
        (await booksRepository.All()).OrderByDescending(book => book.CreatedAt)));

    [HttpGet]
    public IActionResult New() => View();

    [HttpPost]
    [ValidateAntiForgeryToken]
    [TurboStreamResponse]
    public Task<IActionResult> Create(IFormCollection collection) =>
        (
            from formData in ParseFormData(collection)
            from book in booksRepository.Create(formData.Title, formData.Author)
            select book
        )
        .MapAsync<Book, IActionResult>(async book =>
            View(new BooksCreateViewModel(book, (await booksRepository.All()).Count())))
        .OrElse(StatusCode(500, "An unexpected error occurred on the server."));

    [HttpGet]
    public Task<IActionResult> Edit(Guid id) =>
        booksRepository
            .Lookup(id)
            .MapAsync<Book, IActionResult>(book => View(new BooksEditViewModel(book)))
            .OrElse(StatusCode(500, "An unexpected error occurred on the server."));

    [HttpPatch, HttpPut]
    [ValidateAntiForgeryToken]
    public Task<IActionResult> Update(Guid id, IFormCollection collection) =>
        (
            from formData in ParseFormData(collection)
            from book in booksRepository.Lookup(id)
            from updatedBook in booksRepository.Update(
                book with
                {
                    Title = formData.Title,
                    Author = formData.Author
                }
            )
            select updatedBook
        )
        .MapAsync(book => ViewComponentRenderer.RenderAsync("Book", new BooksEditViewModel(book)))
        .OrElse(StatusCode(500, "An unexpected error occurred on the server."));

    [HttpPost]
    [ValidateAntiForgeryToken]
    [TurboStreamResponse]
    public Task<IActionResult> Delete(Guid id) =>
        booksRepository
            .Lookup(id)
            .BindAsync(booksRepository.Delete)
            .MapAsync<Book, IActionResult>(async book =>
                View(new BooksDeleteViewModel(book, (await booksRepository.All()).Count())))
            .OrElse(StatusCode(500, "An unexpected error occurred on the server."));

    private static Task<Maybe<FormData>> ParseFormData(IFormCollection collection) =>
        Task.FromResult(
            from title in collection.JustGetValue("title")
            from author in collection.JustGetValue("author")
            select new FormData(title, author)
        );
}
