using Endava.BookSharing.Domain.Abstractions;

namespace Endava.BookSharing.Application.Models;

public class ImageData : IFileData
{
    public string ContentType { get; set; }
    public byte[] Raw { get; set; }

    public ImageData(string contentType, byte[] raw)
    {
        ContentType = contentType;
        Raw = raw;
    }
}
