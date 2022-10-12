using Endava.BookSharing.Application.Models;
using Endava.BookSharing.Application.Models.Requests;
using MediatR;

namespace Endava.BookSharing.Application.UserManagement.UserSignIn;

public class CheckCredentialsQuery : IRequest<UserRolesDto?>
{
    public CheckCredentialsQuery(UserSignInRequest request)
    {
        UserName = request.UserName;
        Password = request.Password;
    }

    public string UserName { get; }
    public string Password { get; }
}