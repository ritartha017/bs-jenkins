using Endava.BookSharing.Application.Exceptions;
using Endava.BookSharing.Application.UnitTests.Shared;
using Xunit;

namespace Endava.BookSharing.Application.UnitTests.Exceptions;

public class BookSharingGenericExceptionTests
{
    [Theory]
    [AutoMoqData]
    public void Constructor_ShouldSetMessage(string message)
    {
        var exception = new BookSharingGenericException(message);

        Assert.Equal(message, exception.Message);
    }
}
