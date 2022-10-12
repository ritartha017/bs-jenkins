using Endava.BookSharing.Application.Abstract;

namespace Endava.BookSharing.Application.Providers.DateTime;

public class DateTimeProvider : IDateTimeProvider
{
    System.DateTime IDateTimeProvider.Now => System.DateTime.Now;
}