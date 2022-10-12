using Endava.BookSharing.Application.Abstract;
using Endava.BookSharing.Domain.Entities;
using Endava.BookSharing.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Endava.BookSharing.Infrastructure.Repositories;

public class GenreRepository : IGenreRepository
{
    private readonly ApplicationDbContext dbContext;
    private readonly IModelMapper mapper;

    public GenreRepository(ApplicationDbContext dbContext, IModelMapper mapper)
    {
        this.dbContext = dbContext;
        this.mapper = mapper;
    }

    public async Task<ICollection<Genre>> GetGenresByIdsAsync(ICollection<string> ids, CancellationToken cancellationToken)
    {
        return (await dbContext.Genres
            .Where(x => ids.Any(y => x.Id == y))
            .Select(x => new Genre()
            {
                Id = x.Id,
                Name = x.Name
            }).ToListAsync(cancellationToken));
    }

    public async Task<List<Genre>> ListAllAsync(CancellationToken cancellationToken)
    {
        var genresDb = await dbContext.Genres.ToListAsync(cancellationToken);
        var genres = mapper.Map<List<Genre>>(genresDb);

        return genres;
    }
}

