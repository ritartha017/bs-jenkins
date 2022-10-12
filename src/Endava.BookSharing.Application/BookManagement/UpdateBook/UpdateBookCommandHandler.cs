using Endava.BookSharing.Application.Abstract;
using Endava.BookSharing.Application.AuthorManagement.CreateAuthor;
using Endava.BookSharing.Application.Exceptions;
using Endava.BookSharing.Application.FileManagement.CreateFile;
using Endava.BookSharing.Application.Models;
using Endava.BookSharing.Domain.Entities;
using MediatR;
using System.Globalization;

namespace Endava.BookSharing.Application.BookManagement.UpdateBook;

public class UpdateBookCommandHandler : IRequestHandler<UpdateBookCommand, Unit>
{
    private readonly IMediator _mediator;

    private readonly IBookRepository _bookRepository;
    private readonly IAuthorRepository _authorRepository;
    private readonly IGenreRepository _genreRepository;
    private readonly ILanguageRepository _languageRepository;

    private readonly IImageValidator _imageValidator;

    public UpdateBookCommandHandler(IMediator mediator,
        IBookRepository bookRepository,
        IAuthorRepository authorRepository,
        IGenreRepository genreRepository,
        ILanguageRepository languageRepository,
        IImageValidator imageValidator)
    {
        _mediator = mediator;
        _bookRepository = bookRepository;
        _authorRepository = authorRepository;
        _genreRepository = genreRepository;
        _languageRepository = languageRepository;
        _imageValidator = imageValidator;
    }

    public async Task<Unit> Handle(UpdateBookCommand request, CancellationToken cancellationToken)
    {
        Book updatedBook = new Book() { Id = request.BookId };
        var deleteCover = false;

        var bookDataFromDb = await _bookRepository.GetByIdAsync(request.BookId, cancellationToken);

        if (bookDataFromDb is null)
        {
            throw new BookSharingNotFoundException("Book with the given ID not found");
        }

        if (bookDataFromDb!.OwnerId != request.CurrentUser.Id
            && !request.CurrentUser.IsAdminOrSuperAdmin)
        {
            throw new BookSharingAccessDeniedException("Request forbidden");
        }

        if (request.FileType is not null
            && request.RawFile is not null
            && !_imageValidator.IsValidImage(new ImageData(request.FileType, request.RawFile)))
        {
            throw new BookSharingGenericException("File not supported. ");
        }

        updatedBook.Title = request.Title;

        if (!string.IsNullOrEmpty(request.AuthorId))
        {
            bool authorExists = await _authorRepository.GetByIdAsync(request.AuthorId, cancellationToken) is not null;
            if (!authorExists)
            {
                throw new BookSharingNotFoundException("Author with the given ID not found");
            }

            updatedBook.IsDraft = false;
            updatedBook.AuthorId = request.AuthorId;
        }
        else if (!string.IsNullOrEmpty(request.AuthorName))
        {

            string? authorId;

            if (await _authorRepository.IsNameUniqueAsync(request.AuthorName, cancellationToken))
            {
                var createAuthorCommand = new CreateAuthorCommand(request.AuthorName, request.CurrentUser.Id);
                authorId = await _mediator.Send(createAuthorCommand, cancellationToken);
            }
            else
            {
                Author? author = await _authorRepository.GetByNameAsync(request.AuthorName, cancellationToken)!;
                authorId = author!.Id;
            }

            if (authorId is null)
            {
                throw new BookSharingGenericException("Failed to create new author");
            }

            updatedBook.AuthorId = authorId;
            updatedBook.IsDraft = true;
        }
        else throw new BookSharingGenericException("At least Author ID or Author Name must be specified.");

        if (DateTime.TryParseExact(request.PublicationDate, "dd/MM/yyyy", CultureInfo.InvariantCulture,
                                    DateTimeStyles.None, out DateTime parsedResult))
        {
            updatedBook.PublicationDate = parsedResult;
        }
        else throw new BookSharingGenericException("Publication date could not be parsed.");

        if (await _languageRepository.GetByIdAsync(request.LanguageId, cancellationToken) is null)
        {
            throw new BookSharingNotFoundException("Invalid Language ID");
        }
        updatedBook.LanguageId = request.LanguageId;

        ICollection<Genre> genres = await _genreRepository.GetGenresByIdsAsync(request.GenreIds, cancellationToken);
        if (genres is null || genres.Count != request.GenreIds.Count)
        {
            throw new BookSharingNotFoundException("Genres not found with the given IDs");
        }
        updatedBook.Genres = genres;

        if (request.FileType is not null
            && request.RawFile is not null)
        {
            var createCoverCommand = new CreateFileCommand(request.FileType, request.RawFile);
            updatedBook.CoverId = await _mediator.Send(createCoverCommand, cancellationToken);

            if (updatedBook.CoverId is null)
            {
                throw new BookSharingGenericException("Failed to create cover.");
            }
            deleteCover = true;
        }
        else updatedBook.CoverId = bookDataFromDb.CoverId;

        var result = await _bookRepository.UpdateAsync(updatedBook, deleteCover, cancellationToken);

        if (result is false)
        {
            throw new BookSharingGenericException("Failed to update book.");
        }

        return Unit.Value;
    }
}
