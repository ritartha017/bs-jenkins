using System.Runtime.Serialization;

namespace Endava.BookSharing.Application.Exceptions;

[Serializable]
public class BookSharingEntityAlreadyExistException: BookSharingBaseException
{
    public BookSharingEntityAlreadyExistException(string? entity = null)
        : base($"Entity \"{entity}\" already exist!")
    {
    }

    protected BookSharingEntityAlreadyExistException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
    }
}