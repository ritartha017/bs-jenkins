using System.Runtime.Serialization;

namespace Endava.BookSharing.Application.Exceptions
{
    public class BookSharingValidationException : BookSharingBaseException
    {
        public BookSharingValidationException(string? message = null) : base(message)
        {
        }

        protected BookSharingValidationException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }
    }
}