using Endava.BookSharing.Application.Models.Requests;
using Endava.BookSharing.Domain.Entities;
using MediatR;

namespace Endava.BookSharing.Application.UserManagement.UserSignUp;

public class UserSignUpCommand : IRequest<Unit>
{
    public UserSignUpCommand(UserSignUpRequest request)
    {
        User = new User
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            UserName = request.Username
        };

        Password = request.Password;
    }

    public User User { get; set; }
    public string Password { get; set; }
}