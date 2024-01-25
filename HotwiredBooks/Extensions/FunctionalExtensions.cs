using AwesomeResult;
using HotwiredBooks.Errors;
using Microsoft.Extensions.Primitives;

namespace HotwiredBooks.Extensions;

public static class FunctionalExtensions
{
    public static Result<T> ResultNotNull<T>(this T value) =>
        value is not null ? value : new Error().Fail<T>();

    public static Result<StringValues> JustGetValue(this IFormCollection collection, string key) =>
        collection.TryGetValue(key, out var value) ? value : new NotFoundError().Fail<StringValues>();
}
