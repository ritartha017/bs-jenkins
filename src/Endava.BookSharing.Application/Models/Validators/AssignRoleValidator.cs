using Endava.BookSharing.Application.UserManagement.AssignRole;
using FluentValidation;

namespace Endava.BookSharing.Application.Models.Validators;

public class AssignRoleValidator : AbstractValidator<AssignRoleCommand>
{
    public AssignRoleValidator()
    {
        RuleFor(request => request.Id)
            .NotEmpty()
            .WithMessage("Id cannot be empty");
        RuleFor(request => request.Role)
            .NotEmpty()
            .WithMessage("Role cannot be empty");
    }
}