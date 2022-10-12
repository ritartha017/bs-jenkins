using Endava.BookSharing.Application.Abstract;
using Endava.BookSharing.Application.Models.Response;
using MediatR;

namespace Endava.BookSharing.Application.UserManagement.PasswordReset;

public class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand, ResetToken>
{
    private readonly IIdentityService _identityService;

    public ResetPasswordCommandHandler(IIdentityService identityService)
    {
        _identityService = identityService;
    }
    
    public async Task<ResetToken> Handle(ResetPasswordCommand command, CancellationToken cancellationToken)
    {
        var passwordResetToken = await _identityService.CreatePasswordResetToken(command.Email, cancellationToken);
        var resetToken = new ResetToken()
        {
            Hash = passwordResetToken
        };
        return resetToken;
    }
}