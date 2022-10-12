using Endava.BookSharing.Application.Abstract;
using Endava.BookSharing.Application.Exceptions;
using MediatR;

namespace Endava.BookSharing.Application.AuthorManagement.GetAuthorsList;

public class GetAuthorsListQueryHandler : IRequestHandler<GetAuthorsListQuery, List<GetAuthorsListItemResponse>>
{
    private readonly IAuthorRepository _authorRepository;
    private readonly IModelMapper _mapper;

    public GetAuthorsListQueryHandler(IAuthorRepository authorRepository, IModelMapper mapper)
    {
        _authorRepository = authorRepository;
        _mapper = mapper;
    }

    public async Task<List<GetAuthorsListItemResponse>> Handle(GetAuthorsListQuery request, CancellationToken cancellationToken)
    {
        var authors = await _authorRepository.ListAllAsync(cancellationToken);

        if (authors is null)
        {
            throw new BookSharingInternalException();
        }

        var authorsVm = _mapper.Map<List<GetAuthorsListItemResponse>>(authors);

        if (authorsVm is null)
        {
            throw new BookSharingInternalException();
        }

        return authorsVm;
    }
}