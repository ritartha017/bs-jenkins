using MediatR;

namespace Endava.BookSharing.Application.AuthorManagement.GetAuthorsList;

public class GetAuthorsListQuery : IRequest<List<GetAuthorsListItemResponse>>
{
}