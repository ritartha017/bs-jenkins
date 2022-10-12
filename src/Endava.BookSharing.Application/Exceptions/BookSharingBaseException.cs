using System.Runtime.Serialization;

namespace Endava.BookSharing.Application.Exceptions;

[Serializable]
public abstract class BookSharingBaseException : Exception
{
    protected BookSharingBaseException(string? message) : base(string.IsNullOrEmpty(message) ? @"¯\_(ツ)_/¯" : message) { }
        
    protected BookSharingBaseException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
    }
}
