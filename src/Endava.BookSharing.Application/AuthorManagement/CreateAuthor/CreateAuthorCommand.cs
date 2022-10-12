using MediatR;

namespace Endava.BookSharing.Application.AuthorManagement.CreateAuthor;

public class CreateAuthorCommand : IRequest<string>
{
    public CreateAuthorCommand(string fullName, string addedById)
    {
        FullName = fullName;
        AddedById = addedById;
    }

    public string FullName { get; }
    public string AddedById { get; }
}