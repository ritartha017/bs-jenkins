namespace Endava.BookSharing.Infrastructure.Persistence.Models;

public class FileDb
{
    public string Id { get; set; } = null!;
    public string ContentType { get; set; } = null!;
    public byte[] Raw { get; set; } = null!;
}
