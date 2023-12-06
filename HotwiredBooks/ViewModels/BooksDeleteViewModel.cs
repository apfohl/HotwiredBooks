using HotwiredBooks.Models;

namespace HotwiredBooks.ViewModels;

public sealed record BooksDeleteViewModel(Book Book, int NumberOfBooks);
