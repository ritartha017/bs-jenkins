using Endava.BookSharing.Domain.Enums;

namespace Endava.BookSharing.Application.Models;

public class AuthenticatedUser
{
    public string Id { get; }
    public Roles[] Roles { get; }
    public AuthenticatedUser(string id, Roles[] roles)
    {
        Id = id;
        Roles = roles;
    }
    public bool IsAdmin
    {
        get { return Roles.Any(x => x == Domain.Enums.Roles.Admin); }
    }
    public bool IsSuperAdmin
    {
        get { return Roles.Any(x => x == Domain.Enums.Roles.SuperAdmin); }
    }
    public bool IsAdminOrSuperAdmin
    {
        get { return IsAdmin || IsSuperAdmin; }
    }
}
