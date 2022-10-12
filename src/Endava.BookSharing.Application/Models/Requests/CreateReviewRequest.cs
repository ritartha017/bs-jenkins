namespace Endava.BookSharing.Application.Models.Requests;

public class CreateReviewRequest
{
    public string BookId { get; set; } = null!;
    public string Title { get; set; } = null!;
    public string? Content { get; set; } = null;
    public int Rating { get; set; }
}