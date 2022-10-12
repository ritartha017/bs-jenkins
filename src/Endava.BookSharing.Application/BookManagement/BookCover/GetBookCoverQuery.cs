using MediatR;

namespace Endava.BookSharing.Application.BookManagement.BookCover;

public class GetBookCoverQuery : IRequest<Domain.Entities.File?>
{
    public GetBookCoverQuery(string bookId)
    {
        BookId = bookId;
    }

    public string BookId { get; }
}

