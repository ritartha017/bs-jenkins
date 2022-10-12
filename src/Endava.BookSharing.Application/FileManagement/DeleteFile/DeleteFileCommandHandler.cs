using Endava.BookSharing.Application.Abstract;
using Endava.BookSharing.Application.Exceptions;
using Endava.BookSharing.Domain.Enums;
using MediatR;

namespace Endava.BookSharing.Application.FileManagement.DeleteFile;

public class DeleteFileCommandHandler : IRequestHandler<DeleteFileCommand, Unit>
{
    private readonly IBookRepository _bookRepository;

    public DeleteFileCommandHandler(IBookRepository bookRepository)
    {
        _bookRepository = bookRepository;
    }
    
    public async Task<Unit> Handle(DeleteFileCommand request, CancellationToken cancellationToken)
    {
        var bookDataFromDb = await _bookRepository.GetByIdAsync(request.BookId, cancellationToken);
        
        if (bookDataFromDb == null)
            throw new BookSharingGenericException("Invalid Book ID");
        
        var isOwner = bookDataFromDb.OwnerId == request.UserId;
        var isAdmin = Array.Exists(request.UserRoles, roles => roles == Roles.Admin || roles == Roles.SuperAdmin); 
        
        if (!(isOwner || isAdmin))
        {
            throw new BookSharingAccessDeniedException("Request forbidden");
        }

        if (bookDataFromDb.CoverId == null)
            return Unit.Value;

        var deleteCoverResult = await _bookRepository.DeleteCoverAsync(request.BookId, cancellationToken);

        if (deleteCoverResult is false)
            throw new BookSharingGenericException("Book could not be deleted.");

        return Unit.Value;
    }
}