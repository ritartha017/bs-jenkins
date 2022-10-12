using Endava.BookSharing.Domain.Enums;
using MediatR;

namespace Endava.BookSharing.Application.UserManagement.AssignRole;

public class AssignRoleCommand : IRequest<Unit>
{
    public AssignRoleCommand(string id, Roles role)
    {
        Id = id;
        Role = role;
    }
    public string Id { get; }
    public Roles Role { get; }
}