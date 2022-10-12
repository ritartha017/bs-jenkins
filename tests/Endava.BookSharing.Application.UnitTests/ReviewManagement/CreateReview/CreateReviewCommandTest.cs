using Endava.BookSharing.Application.Models.Requests;
using Endava.BookSharing.Application.UnitTests.Shared;
using Endava.BookSharing.Application.UserManagement.BookFeedback;
using FluentAssertions;
using Xunit;

namespace Endava.BookSharing.Application.UnitTests.ReviewManagement.CreateReview
{
    public class CreateReviewCommandTest
    {
        [Theory]
        [AutoMoqData]
        public void CreateObject_WithValidParameter_ValuesHaveBeenAssignedToFields(
        CreateReviewRequest createReviewRequest, string userId)
        {
            var trimedTitle = createReviewRequest.Title.Trim();
            var trimedContent = createReviewRequest.Content!.Trim();
            var result = new CreateReviewCommand(createReviewRequest, userId);

            result.Should().NotBeNull();
            result.Title.Should().Be(trimedTitle);
            result.Content.Should().Be(trimedContent);
            result.UserId.Should().Be(userId);
            result.BookId.Should().Be(createReviewRequest.BookId);
            result.Rating.Should().Be(createReviewRequest.Rating);
        }
    }
}