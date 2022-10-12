namespace Endava.BookSharing.Application.Abstract;

public interface IDateTimeProvider
{
    DateTime Now { get; }
}