using Endava.BookSharing.Domain.Entities;

namespace Endava.BookSharing.Application.Abstract;

public interface IGenreRepository
{
    Task<ICollection<Genre>> GetGenresByIdsAsync(ICollection<string> ids, CancellationToken cancellationToken);
    Task<List<Genre>> ListAllAsync(CancellationToken cancellationToken);
}
