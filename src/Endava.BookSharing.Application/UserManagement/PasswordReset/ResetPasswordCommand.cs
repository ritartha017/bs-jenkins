using Endava.BookSharing.Application.Models.Response;
using MediatR;

namespace Endava.BookSharing.Application.UserManagement.PasswordReset;

public class ResetPasswordCommand : IRequest<ResetToken>
{
    public ResetPasswordCommand(string email)
    {
        Email = email;
    }

    public string  Email { get;}
}