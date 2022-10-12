using Microsoft.AspNetCore.Mvc;

namespace Endava.BookSharing.Application.Models.Requests;

public class FilterBooksRequest
{
    public int Page { get; set; } = 1;
    public string? Genres { get; set; }
    public string? Language { get; set; }

    [FromQuery(Name = "rating_min")]
    public int? RatingMin { get; set; }

    [FromQuery(Name = "rating_max")]
    public int? RatingMax { get; set; }
    public string? Author { get; set; }
}
