using HotwiredBooks.Components;
using Microsoft.AspNetCore.Mvc;

namespace HotwiredBooks.ViewComponents;

public sealed class BooksViewComponent(IBooksRepository booksRepository) : ViewComponent
{
    public async Task<IViewComponentResult> InvokeAsync() =>
        View((await booksRepository.All()).OrderBy(book => book.Title));
}
