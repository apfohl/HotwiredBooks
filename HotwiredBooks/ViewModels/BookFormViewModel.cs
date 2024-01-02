using HotwiredBooks.Models;
using ErrorOr;

namespace HotwiredBooks.ViewModels;

public sealed record BookFormViewModel(ErrorOr<Book> Book);
