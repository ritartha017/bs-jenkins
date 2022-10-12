using Endava.BookSharing.Application.Abstract;
using Endava.BookSharing.Application.Exceptions;
using Endava.BookSharing.Domain.Entities;
using MediatR;

namespace Endava.BookSharing.Application.GenresManagement.GetGenresList;

public class GetGenresListQueryHandler : IRequestHandler<GetGenresListQuery, List<Genre>>
{
    private readonly IGenreRepository _genreRepository;

    public GetGenresListQueryHandler(IGenreRepository genreRepository)
    {
        _genreRepository = genreRepository;
    }

    public async Task<List<Genre>> Handle(GetGenresListQuery request, CancellationToken cancellationToken)
    {
        var genres = await _genreRepository.ListAllAsync(cancellationToken);

        if (genres is null)
        {
            throw new BookSharingInternalException();
        }

        return genres;
    }
}