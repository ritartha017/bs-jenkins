using Microsoft.AspNetCore.Identity;

namespace Endava.BookSharing.Infrastructure.Persistence.Identity.Models;

public class RoleDb : IdentityRole
{
    public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
}