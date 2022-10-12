using Endava.BookSharing.Application.Models.DtoModels;
using Microsoft.AspNetCore.Mvc;

namespace Endava.BookSharing.Application.ReviewManagement.GetReviews;

public class GetReviewsQueryItemsResponse
{
    [FromQuery(Name = "feedbackId")]
    public string FeedbackId { get; set; }
    [FromQuery(Name = "postedBy")]
    public UserDto PostedBy { get; set; }
    [FromQuery(Name = "postedAt")]
    public DateTime PostedAt { get; set; }
    
    public int Rating { get; set; }
    public string Title { get; set; }
    public string? Content { get; set; }
}