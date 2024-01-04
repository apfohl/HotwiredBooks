using Microsoft.Extensions.Primitives;
using ErrorOr;

namespace HotwiredBooks.Extensions;

public static class FunctionalExtensions
{
    public static ErrorOr<T> ToErrorOr<T>(this T value) =>
        value is not null ? ErrorOrFactory.From(value) : ErrorOr<T>.From([Error.Failure()]);

    public static ErrorOr<StringValues> JustGetValue(this IFormCollection collection, string key) =>
        collection.TryGetValue(key, out var value)
            ? ErrorOrFactory.From(value)
            : ErrorOr<StringValues>.From([Error.NotFound()]);

    public static T OrElse<T>(this ErrorOr<T> errorOr, T orElse) =>
        errorOr.Match(value => value, _ => orElse);

    public static T OrElse<T>(this ErrorOr<T> errorOr, Func<T> orElse) =>
        errorOr.Match(value => value, _ => orElse());

    public static async Task<T> OrElse<T>(this Task<ErrorOr<T>> errorOr, Func<T> orElse) =>
        (await errorOr).Match(value => value, _ => orElse());

    public static async Task<T> OrElse<T>(this Task<ErrorOr<T>> errorOr, T orElse) =>
        (await errorOr).Match(value => value, _ => orElse);
}
