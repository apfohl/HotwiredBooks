using Microsoft.Extensions.Primitives;
using MonadicBits;

namespace HotwiredBooks.Extensions;

using static Functional;

public static class FunctionalExtensions
{
    public static Maybe<T> ToMaybe<T>(this T value) => value?.Just() ?? Nothing;

    public static Maybe<StringValues> JustGetValue(this IFormCollection collection, string key) =>
        collection.TryGetValue(key, out var value) ? value.Just() : Nothing;

    public static T OrElse<T>(this Maybe<T> maybe, T orElse) =>
        maybe.Match(value => value, () => orElse);

    public static T OrElse<T>(this Maybe<T> maybe, Func<T> orElse) =>
        maybe.Match(value => value, orElse);

    public static async Task<T> OrElse<T>(this Task<Maybe<T>> maybe, Func<T> orElse) =>
        (await maybe).Match(value => value, orElse);

    public static async Task<T> OrElse<T>(this Task<Maybe<T>> maybe, T orElse) =>
        (await maybe).Match(value => value, () => orElse);
}
