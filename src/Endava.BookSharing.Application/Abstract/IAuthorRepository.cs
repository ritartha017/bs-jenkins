using Endava.BookSharing.Domain.Entities;

namespace Endava.BookSharing.Application.Abstract;

public interface IAuthorRepository
{
    Task<Author?> GetByIdAsync(string id, CancellationToken cancellationToken);
    Task<List<Author>> ListAllAsync(CancellationToken cancellationToken);
    Task<Author?> GetByNameAsync(string authorName, CancellationToken cancellationToken);
    Task<string?> CreateAsync(Author author, CancellationToken cancellationToken);
    Task<bool> IsNameUniqueAsync(string authorName, CancellationToken cancellationToken);
}