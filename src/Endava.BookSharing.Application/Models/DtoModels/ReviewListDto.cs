using Endava.BookSharing.Domain.Entities;

namespace Endava.BookSharing.Application.Models.DtoModels;

public class ReviewListDto
{
    public string FeedbackId { get; set; } = null!;
    public string Title { get; set; } = null!;
    public string? Content { get; set; }
    public int Rating { get; set; }
    public DateTime PostedAt { get; set; }
    public string BookId { get; set; } = null!;
    public UserDto PostedByUser { get; set; } = null!;
}