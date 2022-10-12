using System.Runtime.Serialization;

namespace Endava.BookSharing.Application.Exceptions;

[Serializable]
public class BookSharingAccessDeniedException : BookSharingBaseException
{
    public BookSharingAccessDeniedException(string? message = null) : base(message)
    {
    }

    protected BookSharingAccessDeniedException(SerializationInfo info, StreamingContext context)
        : base(info, context) { }
}
