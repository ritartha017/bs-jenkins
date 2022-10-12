using Endava.BookSharing.Application.Abstract;
using Endava.BookSharing.Application.Exceptions;
using MediatR;

namespace Endava.BookSharing.Application.UserManagement.ResetPassword;

public class UserResetPasswordCommandHandler : IRequestHandler<UserResetPasswordCommand, Unit>
{
    private readonly IIdentityService _identityService;

    public UserResetPasswordCommandHandler(IIdentityService identityService)
    {
        _identityService = identityService;

    }

    public async Task<Unit> Handle(UserResetPasswordCommand request, CancellationToken cancellationToken)
    {
        var result = await _identityService.PasswordResetAsync(request.Email, request.Hash, request.Password, cancellationToken);

        if(result is false)
        {
            throw new BookSharingGenericException("Can't reset password");
        }

        return Unit.Value;
    }
}

