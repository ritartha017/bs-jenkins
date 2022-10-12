using Endava.BookSharing.Application.Abstract;
using Endava.BookSharing.Application.Exceptions;
using MediatR;

namespace Endava.BookSharing.Application.BookManagement.BookCover;

public class GetBookCoverQueryHandler : IRequestHandler<GetBookCoverQuery, Domain.Entities.File?>
{
    private readonly IBookRepository bookRepository;
    private readonly IFileRepository fileRepository;

    public GetBookCoverQueryHandler(IBookRepository bookRepository, IFileRepository fileRepository)
    {
        this.bookRepository = bookRepository;
        this.fileRepository = fileRepository;
    }

    public async Task<Domain.Entities.File?> Handle(GetBookCoverQuery command, CancellationToken cancellationToken)
    {
        var book = await bookRepository.GetByIdAsync(command.BookId, cancellationToken);

        if (book is null)
            throw new BookSharingNotFoundException("Invalid Book ID");

        return await fileRepository.GetByIdAsync(book.CoverId!, cancellationToken);
    }
}

