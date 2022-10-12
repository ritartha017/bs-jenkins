using Endava.BookSharing.Domain.Enums;
using MediatR;

namespace Endava.BookSharing.Application.FileManagement.DeleteFile;

public class DeleteFileCommand : IRequest<Unit>
{
    public DeleteFileCommand(string bookId, string userId, Roles[] userRoles)
    {
        BookId = bookId;
        UserId = userId;
        UserRoles = userRoles;
    }
    public string BookId { get; }
    public string UserId { get; set; }
    public Roles[] UserRoles { get; set; }
}