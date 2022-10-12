using Endava.BookSharing.Domain.Entities;

namespace Endava.BookSharing.Application.Abstract;

public interface ILanguageRepository
{
    Task<List<Language>> ListAllAsync(CancellationToken cancellationToken);
    Task<Language?> GetByNameAsync(string name, CancellationToken cancellationToken);
    Task<Language?> GetByIdAsync(string id, CancellationToken cancellationToken);
}