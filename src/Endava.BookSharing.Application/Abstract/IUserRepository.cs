using Endava.BookSharing.Domain.Entities;
using Endava.BookSharing.Domain.Enums;

namespace Endava.BookSharing.Application.Abstract;

public interface IUserRepository
{
    Task<bool> AddUserToRoleAsync(string userName, Roles role, CancellationToken cancellationToken);
    Task<bool> CreateAsync(User user, string clearPassword, CancellationToken cancellationToken);
    Task<bool> ExistsEmailAsync(string email, CancellationToken cancellationToken);
    Task<bool> ExistsUserNameAsync(string userName, CancellationToken cancellationToken);
    Task<User?> GetByIdAsync(string userId, CancellationToken cancellationToken);
}