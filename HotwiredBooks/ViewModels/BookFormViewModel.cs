using HotwiredBooks.Models;
using MonadicBits;

namespace HotwiredBooks.ViewModels;

public sealed record BookFormViewModel(Maybe<Book> Book);
