using Endava.BookSharing.Application.Models;
using Endava.BookSharing.Application.Models.Response;
using MediatR;

namespace Endava.BookSharing.Application.UserManagement.GetUserDetails;

public class GetUserDetailsQuery : IRequest<UserDetails>
{
    public AuthenticatedUser User { get; }

    public GetUserDetailsQuery(AuthenticatedUser user)
    {
        User = user;
    }
}

