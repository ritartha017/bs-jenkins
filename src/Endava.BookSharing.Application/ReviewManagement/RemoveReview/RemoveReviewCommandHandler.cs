using Endava.BookSharing.Application.Abstract;
using Endava.BookSharing.Application.Exceptions;
using MediatR;

namespace Endava.BookSharing.Application.FeedbackManagement.RemoveFeedback;

public class RemoveReviewCommandHandler : IRequestHandler<RemoveReviewCommand, Unit>
{
    private readonly IReviewRepository reviewRepository;

    public RemoveReviewCommandHandler(IReviewRepository reviewRepository)
    {
        this.reviewRepository = reviewRepository;
    }

    public async Task<Unit> Handle(RemoveReviewCommand request, CancellationToken cancellationToken)
    {
        var result = await reviewRepository.RemoveByIdAsync(request.ReviewId, cancellationToken);

        if (result is false)
        {
            throw new BookSharingNotFoundException("Entity not found");
        }

        return Unit.Value;
    }
}