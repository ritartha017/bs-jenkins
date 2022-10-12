using Endava.BookSharing.Application.UserManagement.ResetPassword;
using Endava.BookSharing.Application.Models.Requests;
using Endava.BookSharing.Application.UnitTests.Shared;
using FluentAssertions;
using Xunit;

namespace Endava.BookSharing.Application.UnitTests.UserManagement.ResetPassword;

public class UserResetPasswordCommandTests
{
    [Theory]
    [AutoMoqData]
    public void CreateObject_WithValidParameter_ValuesHaveBeenAssignedToFields(
        UserResetPasswordRequest request)
    {
        var result = new UserResetPasswordCommand(request);

        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(request, options
            => options.ComparingByMembers<UserSignUpRequest>().ExcludingMissingMembers());
    }
}

