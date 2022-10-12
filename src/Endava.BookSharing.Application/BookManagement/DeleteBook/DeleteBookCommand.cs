using Endava.BookSharing.Application.Models;
using MediatR;

namespace Endava.BookSharing.Application.BookManagement.DeleteBook;

public class DeleteBookCommand : IRequest<Unit>
{
    public DeleteBookCommand(string bookId, AuthenticatedUser currentUser)
    {
        BookId = bookId;
        CurrentUser = currentUser;
    }
    public string BookId { get; }
    public AuthenticatedUser CurrentUser { get; set; }
}
