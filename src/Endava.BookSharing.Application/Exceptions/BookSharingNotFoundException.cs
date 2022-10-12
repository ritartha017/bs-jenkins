namespace Endava.BookSharing.Application.Exceptions;

#pragma warning disable S3925 // "ISerializable" should be implemented correctly
public class BookSharingNotFoundException : BookSharingBaseException
{
    public BookSharingNotFoundException(string message) : base(message) { }
}
#pragma warning restore S3925 // "ISerializable" should be implemented correctly
