using Endava.BookSharing.Application.UserManagement.PasswordReset;
using Endava.BookSharing.Application.UnitTests.Shared;
using Xunit;

namespace Endava.BookSharing.Application.UnitTests.UserManagement.PasswordReset;

public class ResetPasswordCommandTests
{
    [Theory]
    [AutoMoqData]
    public void Constructor_ShouldSetProperties(string email)
    {
        var result = new ResetPasswordCommand(email);

        Assert.Equal(email, result.Email);
    }
}