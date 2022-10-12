using System.Runtime.Serialization;

namespace Endava.BookSharing.Application.Exceptions;

[Serializable]
public class BookSharingGenericException : BookSharingBaseException
{
    public BookSharingGenericException(string message) : base(message)
    {
    }

    protected BookSharingGenericException(SerializationInfo info, StreamingContext context)
        : base(info, context) { }
}