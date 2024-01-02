using ErrorOr;

namespace HotwiredBooks.Extensions;

public static class ErrorOrExtensions
{
    public static ErrorOr<T> Success<T>(this T value) =>
        ErrorOrFactory.From(value);
}
