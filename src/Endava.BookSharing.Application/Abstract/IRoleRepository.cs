using Endava.BookSharing.Domain.Enums;

namespace Endava.BookSharing.Application.Abstract;

public interface IRoleRepository
{
    Task<bool> CreateAsync(Roles role, CancellationToken cancellationToken);

    Task<bool> ExistsAsync(Roles role, CancellationToken cancellationToken);
}