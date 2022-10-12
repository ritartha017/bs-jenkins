using System.Runtime.Serialization;

namespace Endava.BookSharing.Application.Exceptions;

[Serializable]
public class BookSharingInvalidCredentialsException : BookSharingGenericException
{
    public BookSharingInvalidCredentialsException() : base("Invalid username or password.")
    {
    }

    protected BookSharingInvalidCredentialsException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
    }
}