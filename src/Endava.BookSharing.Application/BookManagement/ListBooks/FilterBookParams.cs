namespace Endava.BookSharing.Application.BookManagement.ListBooks;

public class FilterBookParams
{
    public string? Language { get; set; }
    public ICollection<string> Genres { get; set; } = new List<string>();
    public int RatingMin { get; set; }
    public int RatingMax { get; set; }
    public string? Author { get; set; }
    public bool IsRatingSpecified { get; set; } = false;

}
