namespace Endava.BookSharing.Application.Extensions;

public static class DoubleExtensions
{
    public static bool IsBetween(this double number, int min, int max)
        => (number >= min && number <= max);
}
