using Endava.BookSharing.Application.Abstract;
using Endava.BookSharing.Application.Exceptions;
using MediatR;

namespace Endava.BookSharing.Application.UserManagement.ResetPassword;

public class UserVerifyResetPasswordHashCommandHandler : IRequestHandler<UserVerifyResetPasswordHashCommand, Unit>
{
    private readonly IIdentityService _identityService;

    public UserVerifyResetPasswordHashCommandHandler(IIdentityService identityService)
    {
        _identityService = identityService;

    }

    public async Task<Unit> Handle(UserVerifyResetPasswordHashCommand request, CancellationToken cancellationToken)
    {
        var result = await _identityService.VerifyResetPasswordHash(request.Email, request.Hash, cancellationToken);

        if (result is false)
        {
            throw new BookSharingGenericException("Reset password hash is not valid");
        }

        return Unit.Value;
    }
}

