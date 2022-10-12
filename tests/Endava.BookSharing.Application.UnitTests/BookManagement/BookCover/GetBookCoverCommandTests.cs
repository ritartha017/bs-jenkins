using Endava.BookSharing.Application.BookManagement.BookCover;
using Endava.BookSharing.Application.UnitTests.Shared;
using Xunit;

namespace Endava.BookSharing.Application.UnitTests.BookManagement.BookCover;


public class GetBookCoverCommandTests
{
    [Theory]
    [AutoMoqData]
    public void Constructor_ShouldSetProperty(string bookId)
    {
        var result = new GetBookCoverQuery(bookId);

        Assert.Equal(bookId, result.BookId);
    }
}

