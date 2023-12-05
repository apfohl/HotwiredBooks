using HotwiredBooks.Components;
using HotwiredBooks.Models;
using Microsoft.AspNetCore.Mvc;

namespace HotwiredBooks.Controllers;

public sealed class BooksController(IBooksRepository booksRepository) : Controller
{
    // GET: BooksController
    public async Task<ActionResult> Index() => View(
        new BooksViewModel((await booksRepository.All()).OrderBy(book => book.Title))
    );

    // GET: BooksController/Create
    public ActionResult Create()
    {
        return View();
    }

    // POST: BooksController/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Create(IFormCollection collection)
    {
        try
        {
            return RedirectToAction(nameof(Index));
        }
        catch
        {
            return View();
        }
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
}
