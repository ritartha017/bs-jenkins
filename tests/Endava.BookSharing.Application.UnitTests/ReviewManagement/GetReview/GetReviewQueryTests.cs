using Endava.BookSharing.Application.ReviewManagement.GetReviews;
using Endava.BookSharing.Application.UnitTests.Shared;
using FluentAssertions;
using Xunit;

namespace Endava.BookSharing.Application.UnitTests.ReviewManagement.GetReviews;

public class GetReviewQueryTests
{
    [Theory]
    [AutoMoqData]
    public void CreateObject_WithValidParameter_ValuesHaveBeenAssignedToFields(string bookId, int page)
    {
        var result = new GetReviewsQuery(bookId, page);

        result.Should().NotBeNull();
        Assert.Equal(bookId, result.BookId);
        Assert.Equal(page, result.Page);
    }
}

