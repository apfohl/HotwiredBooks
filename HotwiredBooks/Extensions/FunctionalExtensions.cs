using MonadicBits;

namespace HotwiredBooks.Extensions;

using static Functional;

public static class FunctionalExtensions
{
    public static Maybe<T> ToMaybe<T>(this T value) => value?.Just() ?? Nothing;
}
