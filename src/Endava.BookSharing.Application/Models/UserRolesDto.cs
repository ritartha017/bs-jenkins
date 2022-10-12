namespace Endava.BookSharing.Application.Models;

public class UserRolesDto
{
    public UserRolesDto(string userId, IEnumerable<string> userRoles)
    {
        UserId = userId;
        UserRoles = userRoles;
    }

    public string UserId { get; }

    public IEnumerable<string> UserRoles { get; }
}
