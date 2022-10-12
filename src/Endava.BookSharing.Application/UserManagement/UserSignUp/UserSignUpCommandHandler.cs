using Endava.BookSharing.Application.Abstract;
using Endava.BookSharing.Application.Exceptions;
using Endava.BookSharing.Domain.Enums;
using MediatR;

namespace Endava.BookSharing.Application.UserManagement.UserSignUp;

public class UserSignUpCommandHandler : IRequestHandler<UserSignUpCommand, Unit>
{
    private readonly IIdentityService _identityService;
    private readonly IUserRepository _userRepository;

    public UserSignUpCommandHandler(IIdentityService identityService, IUserRepository userRepository)
    {
        _identityService = identityService;
        _userRepository = userRepository;
    }

    public async Task<Unit> Handle(UserSignUpCommand command, CancellationToken cancellationToken)
    {
        var emailIsAvailable = await _userRepository.ExistsEmailAsync(command.User.Email, cancellationToken);
        if (emailIsAvailable is not false)
        {
            throw new BookSharingGenericException
                ("Your registration failed because such an email exists in the system.");
        }

        var userNameIsAvailable = await _userRepository.ExistsUserNameAsync(command.User.UserName, cancellationToken);
        if (userNameIsAvailable is not false)
        {
            throw new BookSharingGenericException
                ("Your registration failed because such an user name exists in the system.");
        }

        var result = await _userRepository.CreateAsync(command.User, command.Password, cancellationToken);

        if (result is false)
        {
            throw new BookSharingGenericException
                ("Your registration failed due to an internal server error. Please try again later.");
        }

        await _identityService.CreateRoleIfNotExists(Roles.User, cancellationToken);

        result = await _identityService.AddUserToRole(command.User, Roles.User, cancellationToken);
        if (result is false)
        {
            throw new BookSharingGenericException
                ("Your registration failed due to an internal server error. Please try again later.");
        }

        return Unit.Value;
    }
}