using Endava.BookSharing.Domain.Abstractions;

namespace Endava.BookSharing.Application.Abstract;

public interface IFileRepository
{
    Task<Domain.Entities.File?> GetByBytesAsync(IFileData file, CancellationToken cancellationToken);
    Task<string?> CreateAsync(Domain.Entities.File file, CancellationToken cancellationToken);
    Task<Domain.Entities.File?> GetByIdAsync(string id, CancellationToken cancellationToken);
}