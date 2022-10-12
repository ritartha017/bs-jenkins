using Endava.BookSharing.Application.Models.Requests;
using Endava.BookSharing.Application.UnitTests.Shared;
using Endava.BookSharing.Application.UserManagement.UserSignIn;
using FluentAssertions;
using Xunit;

namespace Endava.BookSharing.Application.UnitTests.UserManagement.SignInUser
{
    public class CheckCredentialsQueryTests
    {
        [Theory]
        [AutoMoqData]
        public void CreateObject_WithValidParameter_ValuesHaveBeenAssignedToFields(
        UserSignInRequest userSignInRequest)
        {
            var result = new CheckCredentialsQuery(userSignInRequest);

            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(userSignInRequest, options
                => options.ComparingByMembers<UserSignInRequest>().ExcludingMissingMembers());
        }
    }
}