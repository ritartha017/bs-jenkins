using Endava.BookSharing.Application.Exceptions;
using Xunit;

namespace Endava.BookSharing.Application.UnitTests.Exceptions;

public class BookSharingInvalidCredentialsExceptionTests
{
    [Fact]
    public void Constructor_ShouldSetDefaultMessage()
    {
        var exception = new BookSharingInvalidCredentialsException();

        Assert.Equal("Invalid username or password.", exception.Message);
    }
}
