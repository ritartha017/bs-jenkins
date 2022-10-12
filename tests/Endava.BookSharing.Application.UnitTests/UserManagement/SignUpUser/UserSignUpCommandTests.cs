using Endava.BookSharing.Application.Models.Requests;
using Endava.BookSharing.Application.UnitTests.Shared;
using Endava.BookSharing.Application.UserManagement.UserSignUp;
using FluentAssertions;
using Xunit;

namespace Endava.BookSharing.Application.UnitTests.UserManagement.SignUpUser;

public class UserSignUpCommandTests
{
    [Theory]
    [AutoMoqData]
    public void CreateObject_WithValidParameter_ValuesHaveBeenAssignedToFields(
        UserSignUpRequest userSignUpRequest)
    {
        var result = new UserSignUpCommand(userSignUpRequest);

        result.User.Should().BeEquivalentTo(userSignUpRequest, options
            => options.ComparingByMembers<UserSignUpRequest>().ExcludingMissingMembers());
    }
}