using AwesomeResult;
using HotwiredBooks.Models;

namespace HotwiredBooks.ViewModels;

public sealed record BookFormViewModel(Result<Book> Book);
