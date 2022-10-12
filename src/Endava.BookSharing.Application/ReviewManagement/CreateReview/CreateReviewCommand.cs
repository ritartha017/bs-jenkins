using Endava.BookSharing.Application.Models.Requests;
using MediatR;

namespace Endava.BookSharing.Application.UserManagement.BookFeedback;

public class CreateReviewCommand : IRequest<Unit>
{
    public CreateReviewCommand(CreateReviewRequest request, string currentUserId)
    {
        BookId = request.BookId;
        Title = request.Title.Trim();
        Content = request.Content?.Trim();
        Rating = request.Rating;
        UserId = currentUserId;
    }

    public string BookId { get; }
    public string Title { get; }
    public string? Content { get; }
    public int Rating { get; }
    public string UserId { get; }
}