using System.Runtime.Serialization;

namespace Endava.BookSharing.Application.Exceptions;

[Serializable]
public class BookSharingInternalException: BookSharingBaseException
{
    public BookSharingInternalException()
        : base("Your operation failed due to an internal server error. Please try again later.")
    {
    }

    protected BookSharingInternalException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
    }
}