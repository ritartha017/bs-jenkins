using Endava.BookSharing.Application.Models.Validators;
using Endava.BookSharing.Application.UnitTests.Shared.Extensions;
using Endava.BookSharing.Application.UserManagement.AssignRole;
using Endava.BookSharing.Domain.Enums;
using FluentValidation.Results;
using Xunit;

namespace Endava.BookSharing.Application.UnitTests.Models.Validators;

public class AssignRoleValidatorTests
{
    private ValidationResult? RunValidation(AssignRoleCommand command)
    {
        var validator = new AssignRoleValidator();
        var result = validator.Validate(command);
        return result;
    }
    
    [Theory]
    [InlineData("", Roles.Admin)]
    [InlineData(null, Roles.Admin)]
    public void Validate_Id_EmptyOrNullId_DoesntAllowEmpty(string id, Roles role)
    {
        var result = RunValidation(new AssignRoleCommand(id, role));
        result.AssertFailedValidation("Id cannot be empty");
    }
}