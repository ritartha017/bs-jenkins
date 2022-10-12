#pragma warning disable S3358 // Ternary operators should not be nested
using Endava.BookSharing.Application.Abstract;
using Endava.BookSharing.Application.Exceptions;
using Endava.BookSharing.Application.Models.Response;
using MediatR;

namespace Endava.BookSharing.Application.UserManagement.GetUserDetails;

public class GetUserDetailsQueryHandler : IRequestHandler<GetUserDetailsQuery, UserDetails>
{
    private readonly IUserRepository userRepository;

    public GetUserDetailsQueryHandler(IUserRepository userRepository)
    {
        this.userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
    }

    public async Task<UserDetails> Handle(GetUserDetailsQuery query, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByIdAsync(query.User.Id, cancellationToken);
        if (user == null)
        {
            throw new BookSharingNotFoundException("Trying to get non-existent user");
        }

        string role =
            query.User.IsAdmin? "Admin"
          : query.User.IsSuperAdmin ? "SuperAdmin"
          : "User";

        var userDetails = new UserDetails()
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            UserName = user.UserName,
            Email = user.Email,
            Role = role,
        };
        return userDetails;
    }
}
#pragma warning restore S3358 // Ternary operators should not be nested

