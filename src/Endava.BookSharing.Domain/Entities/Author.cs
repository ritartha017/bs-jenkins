namespace Endava.BookSharing.Domain.Entities;

public class Author
{
    public string Id { get; set; } = null!;
    public string FullName { get; set; } = null!;
    public bool IsApproved { get; set; }
    public string AddedByUserId { get; set; } = null!;
}
