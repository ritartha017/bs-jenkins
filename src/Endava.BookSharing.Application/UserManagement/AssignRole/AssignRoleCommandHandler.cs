using Endava.BookSharing.Application.Abstract;
using Endava.BookSharing.Application.Exceptions;
using Endava.BookSharing.Domain.Enums;
using MediatR;

namespace Endava.BookSharing.Application.UserManagement.AssignRole;

public class AssignRoleCommandHandler : IRequestHandler<AssignRoleCommand, Unit>
{
    private readonly IIdentityService _identityService;

    public AssignRoleCommandHandler(IIdentityService identityService)
    {
        _identityService = identityService;
    }

    public async Task<Unit> Handle(AssignRoleCommand request, CancellationToken cancellationToken)
    {
        if (request.Role == Roles.SuperAdmin)
        {
            throw new BookSharingAccessDeniedException("Cannot set this role");
        }

        var isHaveRole = await _identityService.IsRoleAssigned(request.Id, request.Role, cancellationToken);
        if (isHaveRole)
        {
            throw new BookSharingEntityAlreadyExistException("role");
        }

        var isRoleAssigned = await _identityService.UpdateRole(request.Id, request.Role, cancellationToken);
        if (isRoleAssigned is false)
        {
            throw new BookSharingInternalException();
        }

        return Unit.Value;
    }
}