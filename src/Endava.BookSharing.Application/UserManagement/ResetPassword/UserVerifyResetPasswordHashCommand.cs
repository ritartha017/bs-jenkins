using Endava.BookSharing.Application.Models.Requests;
using MediatR;

namespace Endava.BookSharing.Application.UserManagement.ResetPassword;

public class UserVerifyResetPasswordHashCommand : IRequest<Unit>
{
    public UserVerifyResetPasswordHashCommand(UserVerifyResetPasswordHashRequest request)
    {
        Email = request.Email;
        Hash = request.Hash;
    }

    public string Email { get;}
    public string Hash { get; }
}

