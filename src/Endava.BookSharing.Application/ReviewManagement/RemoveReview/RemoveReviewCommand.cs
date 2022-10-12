using MediatR;

namespace Endava.BookSharing.Application.FeedbackManagement.RemoveFeedback;

public class RemoveReviewCommand : IRequest<Unit>
{
    public RemoveReviewCommand(string reviewId)
    {
        ReviewId = reviewId;
    }
    public string ReviewId { get;}
}