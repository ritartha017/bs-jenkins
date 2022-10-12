using Endava.BookSharing.Application.Models;
using Endava.BookSharing.Application.Models.Response;
using MediatR;

namespace Endava.BookSharing.Application.BookManagement.GetBook;

public class GetBookCommand : IRequest<BookResponse>
{
    public GetBookCommand(string bookId, AuthenticatedUser authenticatedUser)
    {
        BookId = bookId;
        AuthenticatedUser = authenticatedUser;
    }

    public string BookId { get; set; }
    public AuthenticatedUser AuthenticatedUser { get; set; }
}