using Endava.BookSharing.Application.Exceptions;
using Endava.BookSharing.Application.Models;
using Endava.BookSharing.Domain.Enums;
using System.Security.Claims;

namespace Endava.BookSharing.Presentation.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static string GetUserId(this ClaimsPrincipal user)
    {
        var userId = user.Claims.FirstOrDefault(x => x.Type == Consts.UserIdClaimName)?.Value;
        if (string.IsNullOrEmpty(userId)) throw new BookSharingAccessDeniedException("Access forbidden");
        return userId;
    }

    public static Roles[] GetUserRoles(this ClaimsPrincipal user)
    {
        var roles = user.Claims.FirstOrDefault(x => x.Type == Consts.RolesClaimName)?.Value;
        if (string.IsNullOrEmpty(roles)) throw new BookSharingAccessDeniedException("Access forbidden");

        List<Roles> userRoles = new List<Roles>();
        foreach (var role in roles.Split(','))
        {
            bool parsed = Enum.TryParse<Roles>(role, out Roles roleValue);
            if (!parsed) continue;

            userRoles.Add(roleValue);
        }

        if (!userRoles.Any()) throw new BookSharingAccessDeniedException("Access forbidden");
        return userRoles.ToArray();
    }

    public static AuthenticatedUser GetCurrentAuthenticatedUserData(this ClaimsPrincipal user)
    {
        var userId = user.GetUserId();
        var roles = user.GetUserRoles();

        return new AuthenticatedUser(userId, roles);
    }
}