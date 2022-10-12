using Endava.BookSharing.Application.Abstract;
using Endava.BookSharing.Application.Exceptions;
using MediatR;

namespace Endava.BookSharing.Application.BookManagement.DeleteBook;

public class DeleteBookCommandHandler : IRequestHandler<DeleteBookCommand, Unit>
{
    private readonly IBookRepository _bookRepository;

    public DeleteBookCommandHandler(IBookRepository bookRepository)
    {
        _bookRepository = bookRepository;
    }

    public async Task<Unit> Handle(DeleteBookCommand request, CancellationToken cancellationToken)
    {
        var bookDataFromDb = await _bookRepository.GetByIdAsync(request.BookId, cancellationToken);

        if (string.IsNullOrEmpty(request.BookId) || bookDataFromDb is null)
        {
            throw new BookSharingGenericException("Invalid Book ID");
        }

        if (bookDataFromDb.OwnerId != request.CurrentUser.Id
            && !request.CurrentUser.IsAdminOrSuperAdmin)
        {
            throw new BookSharingAccessDeniedException("Request forbidden");
        }

        var deleteBookResult = await _bookRepository.DeleteAsync(request.BookId, cancellationToken);

        if (deleteBookResult is false)
            throw new BookSharingGenericException("Book could not be deleted.");

        return Unit.Value;
    }
}
