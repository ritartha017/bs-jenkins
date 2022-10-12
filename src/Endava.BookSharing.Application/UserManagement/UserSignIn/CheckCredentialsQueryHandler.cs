using Endava.BookSharing.Application.Abstract;
using Endava.BookSharing.Application.Models;
using MediatR;

namespace Endava.BookSharing.Application.UserManagement.UserSignIn;

public class CheckCredentialsQueryHandler : IRequestHandler<CheckCredentialsQuery, UserRolesDto?>
{
    private readonly IIdentityService identityService;

    public CheckCredentialsQueryHandler(IIdentityService identityService)
    {
        this.identityService = identityService;
    }

    public Task<UserRolesDto?> Handle(CheckCredentialsQuery query, CancellationToken cancellationToken)
    {
        return identityService.ValidateUserCredentialsAsync(query.UserName, query.Password, cancellationToken);
    }
}