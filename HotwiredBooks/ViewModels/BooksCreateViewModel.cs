using HotwiredBooks.Models;

namespace HotwiredBooks.ViewModels;

public sealed record BooksCreateViewModel(Book Book, int NumberOfBooks);
