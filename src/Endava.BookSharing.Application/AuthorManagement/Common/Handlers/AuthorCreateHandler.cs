using Endava.BookSharing.Application.Abstract;
using Endava.BookSharing.Application.Exceptions;
using Endava.BookSharing.Domain.Entities;

namespace Endava.BookSharing.Application.AuthorManagement.Common.Handlers;

public class AuthorCreateHandler : AbstractHandler<Author>
{
    private readonly IAuthorRepository _authorRepository;
    public AuthorCreateHandler(IAuthorRepository authorRepository)
    {
        _authorRepository = authorRepository;
    }

    public override async Task ProcessRequestAsync(Author author, CancellationToken cancellationToken)
    {
        var authorId = await _authorRepository.CreateAsync(author, cancellationToken);

        if (authorId is null)
        {
            throw new BookSharingInternalException();
        }

        Data = authorId;
    }
}