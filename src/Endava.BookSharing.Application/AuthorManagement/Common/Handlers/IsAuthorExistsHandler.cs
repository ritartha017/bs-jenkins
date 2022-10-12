using Endava.BookSharing.Application.Abstract;
using Endava.BookSharing.Application.Exceptions;
using Endava.BookSharing.Domain.Entities;

namespace Endava.BookSharing.Application.AuthorManagement.Common.Handlers;

public class IsAuthorExistsHandler : AbstractHandler<Author>
{
    private readonly IAuthorRepository _authorRepository;

    public IsAuthorExistsHandler(IAuthorRepository authorRepository)
    {
        _authorRepository = authorRepository;
    }

    public override async Task ProcessRequestAsync(Author author, CancellationToken cancellationToken)
    {
        var entity = await _authorRepository.GetByNameAsync(author.FullName, cancellationToken);

        if (entity is not null)
        {
            throw new BookSharingEntityAlreadyExistException(entity.FullName);
        }

        await successor.ProcessRequestAsync(author, cancellationToken);
    }
}