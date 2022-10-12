using Endava.BookSharing.Application.Abstract;
using Endava.BookSharing.Application.Exceptions;
using Endava.BookSharing.Domain.Entities;

namespace Endava.BookSharing.Application.AuthorManagement.Common.Handlers;

public class IsUserExistsHandler : AbstractHandler<Author>
{
    private readonly IUserRepository _userRepository;

    public IsUserExistsHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public override async Task ProcessRequestAsync(Author author, CancellationToken cancellationToken)
    {
        var entity = await _userRepository.GetByIdAsync(author.AddedByUserId, cancellationToken);

        if (entity is null)
        {
            throw new BookSharingInternalException();
        }

        await successor.ProcessRequestAsync(author, cancellationToken);
    }
}