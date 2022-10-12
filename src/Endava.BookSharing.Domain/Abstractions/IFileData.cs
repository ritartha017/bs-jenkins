namespace Endava.BookSharing.Domain.Abstractions;

public interface IFileData
{
    public string ContentType { get; set; }
    public byte[] Raw { get; set; }
}
