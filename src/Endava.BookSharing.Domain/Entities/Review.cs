namespace Endava.BookSharing.Domain.Entities;

public class Review
{
    public string Id { get; set; } = null!;
    public string Title { get; set; } = null!;
    public string? Content { get; set; }
    public int Rating { get; set; }
    public DateTime PostedAt { get; set; }
    public string BookId { get; set; } = null!;
    public string PostedByUserId { get; set; } = null!;
}