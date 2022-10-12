using Endava.BookSharing.Application.Abstract;
using Endava.BookSharing.Application.AuthorManagement.CreateAuthor;
using Endava.BookSharing.Application.Exceptions;
using Endava.BookSharing.Domain.Entities;
using System.Globalization;
using MediatR;

namespace Endava.BookSharing.Application.BookManagement.CreateBook;

public class CreateBookCommandHandler : IRequestHandler<CreateBookCommand, Unit>
{
    private readonly IBookRepository bookRepository;
    private readonly IAuthorRepository authorRepository;
    private readonly IGenreRepository genreRepository;
    private readonly ILanguageRepository languageRepository;
    private readonly IFileRepository fileRepository;
    private readonly IMediator mediator;

    public CreateBookCommandHandler(
        IBookRepository bookRepository,
        IAuthorRepository authorRepository,
        IGenreRepository genreRepository,
        ILanguageRepository languageRepository,
        IFileRepository fileRepository,
        IMediator mediator)
    {
        this.bookRepository = bookRepository ?? throw new ArgumentNullException(nameof(bookRepository));
        this.authorRepository = authorRepository ?? throw new ArgumentNullException(nameof(authorRepository));
        this.genreRepository = genreRepository ?? throw new ArgumentNullException(nameof(genreRepository));
        this.languageRepository = languageRepository ?? throw new ArgumentNullException(nameof(languageRepository));
        this.fileRepository = fileRepository ?? throw new ArgumentNullException(nameof(fileRepository));
        this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    public async Task<Unit> Handle(CreateBookCommand command, CancellationToken cancellationToken)
    {
        bool authorIdIsSpecified = !string.IsNullOrEmpty(command.AuthorId);
        bool authorFullNameIsSpecified = !string.IsNullOrEmpty(command.AuthorFullName);
        bool isDraft = false;
        string authorId = string.Empty;

        if (authorIdIsSpecified)
        {
            Author? author = await authorRepository.GetByIdAsync(command.AuthorId!, cancellationToken);
            if (author is null)
                throw new BookSharingNotFoundException("Invalid Author ID");
            authorId = author.Id;
        }
        else if (authorFullNameIsSpecified)
        {
            var author = new CreateAuthorCommand(command.AuthorFullName!, command.CurrentUserId);
            authorId = await mediator.Send(author, cancellationToken);
            if (authorId is null)
                throw new BookSharingGenericException("Failed to create new author");
            isDraft = true;
        }

        var genres = await genreRepository.GetGenresByIdsAsync(command.GenreIds, cancellationToken);
        if (genres is null || genres.Count != command.GenreIds.Count)
            throw new BookSharingNotFoundException("Genres not found with the given IDs");

        if (await languageRepository.GetByIdAsync(command.LanguageId, cancellationToken) is null)
            throw new BookSharingNotFoundException("Invalid Language ID");

        const string? datePattern = "MM/dd/yyyy";
        if (!DateTime.TryParseExact(command.PublicationDate, datePattern, CultureInfo.InvariantCulture,
                                    DateTimeStyles.None, out DateTime publicationDate))
            throw new BookSharingGenericException("Publication date could not be parsed");

        var newCover = new Domain.Entities.File() { Id = Guid.NewGuid().ToString(), Data = command.CoverData };
        var coverId = await fileRepository.CreateAsync(newCover, cancellationToken);
       
        var newBook = new Book()
        {
            Id = Guid.NewGuid().ToString(),
            Title = command.Title,
            OwnerId = command.CurrentUserId,
            AuthorId = authorId,
            IsDraft = isDraft,
            PublicationDate = publicationDate,
            Genres = genres,
            LanguageId = command.LanguageId,
            CoverId = coverId!,
        };
        if (await bookRepository.CreateAsync(newBook, cancellationToken) is false)
            throw new BookSharingGenericException("Failed to create new book.");

        return Unit.Value;
    }
}
