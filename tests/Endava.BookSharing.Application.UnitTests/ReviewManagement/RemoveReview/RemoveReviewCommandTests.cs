using Endava.BookSharing.Application.FeedbackManagement.RemoveFeedback;
using Endava.BookSharing.Application.UnitTests.Shared;
using Xunit;

namespace Endava.BookSharing.Application.UnitTests.FeedbackManagement.RemoveFeedback;

public class RemoveReviewCommandTests
{
    [Theory]
    [AutoMoqData]
    public void Constructor_ShouldSetProperties(string id)
    {
        var result = new RemoveReviewCommand(id);

        Assert.Equal(id, result.ReviewId);
    }
}