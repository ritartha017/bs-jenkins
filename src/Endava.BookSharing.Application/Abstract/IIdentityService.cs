using Endava.BookSharing.Application.Models;
using Endava.BookSharing.Domain.Entities;
using Endava.BookSharing.Domain.Enums;

namespace Endava.BookSharing.Application.Abstract;

public interface IIdentityService
{
    Task<bool> AddUserToRole(User user, Roles role, CancellationToken cancellationToken);
    Task CreateRoleIfNotExists(Roles role, CancellationToken cancellationToken);
    Task<bool> UpdateRole(string id, Roles role, CancellationToken cancellationToken);
    Task<bool> IsRoleAssigned(string id, Roles role, CancellationToken cancellationToken);
    Task<UserRolesDto?> ValidateUserCredentialsAsync(string username, string password, CancellationToken cancellationToken);
    Task<string> CreatePasswordResetToken(string email, CancellationToken cancellationToken);
    Task<bool> VerifyResetPasswordHash(string email, string hash, CancellationToken cancellationToken);

    Task<bool> PasswordResetAsync(string email, string hash, string password, CancellationToken cancellationToken);
}