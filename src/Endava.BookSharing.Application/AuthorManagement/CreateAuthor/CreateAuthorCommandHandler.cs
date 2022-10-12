using Endava.BookSharing.Application.Abstract;
using Endava.BookSharing.Application.AuthorManagement.Common.Handlers;
using Endava.BookSharing.Domain.Entities;
using MediatR;

namespace Endava.BookSharing.Application.AuthorManagement.CreateAuthor;

public class CreateAuthorCommandHandler : IRequestHandler<CreateAuthorCommand, string>
{
    private readonly AbstractHandler<Author> _authorExists;
    private readonly AbstractHandler<Author> _userExists;
    private readonly AbstractHandler<Author> _authorCreate;

    public CreateAuthorCommandHandler(IAuthorRepository authorRepository, IUserRepository userRepository)
    {
        _authorExists = new IsAuthorExistsHandler(authorRepository);
        _userExists = new IsUserExistsHandler(userRepository);
        _authorCreate = new AuthorCreateHandler(authorRepository);
    }

    public async Task<string> Handle(CreateAuthorCommand request, CancellationToken cancellationToken)
    {
        var author = new Author
        {
            FullName = request.FullName,
            AddedByUserId = request.AddedById,
            IsApproved = false
        };

        _authorExists.SetSuccessor(_userExists);
        _userExists.SetSuccessor(_authorCreate);

        await _authorExists.ProcessRequestAsync(author, cancellationToken);

        return _authorCreate.Data;
    }
}