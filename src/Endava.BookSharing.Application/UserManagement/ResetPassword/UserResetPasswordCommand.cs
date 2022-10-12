using Endava.BookSharing.Application.Models.Requests;
using MediatR;

namespace Endava.BookSharing.Application.UserManagement.ResetPassword;

public class UserResetPasswordCommand : IRequest<Unit>
{
    public UserResetPasswordCommand(UserResetPasswordRequest request)
    {
        Email = request.Email;
        Hash = request.Hash;
        Password = request.Password;
    }

    public string Email { get;}
    public string Hash { get; }
    public string Password { get; }
}

