using Microsoft.AspNetCore.Identity;

namespace Endava.BookSharing.Infrastructure.Persistence.Identity.Models;

public class UserDb : IdentityUser
{
    public string FirstName { get; init; } = null!;
    public string LastName { get; init; } = null!;

    public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
}