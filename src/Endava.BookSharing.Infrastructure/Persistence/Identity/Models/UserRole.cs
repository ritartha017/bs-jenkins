using Microsoft.AspNetCore.Identity;

namespace Endava.BookSharing.Infrastructure.Persistence.Identity.Models;

public class UserRole : IdentityUserRole<string>
{
    public virtual UserDb UserDb { get; set; } = null!;
    public virtual RoleDb RoleDb { get; set; } = null!;
}