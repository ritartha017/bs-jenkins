using Endava.BookSharing.Domain.Abstractions;

namespace Endava.BookSharing.Domain.Entities;

public class File
{
    public string Id { get; set; } = null!;
    public IFileData Data { get; set; } = null!;
}
