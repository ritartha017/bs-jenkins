
namespace Endava.BookSharing.Application.Extensions;

public static class StreamExtensions
{
    public static byte[] ToByteArray(this Stream stream)
    {
        var memoryStream = new MemoryStream();
        stream.CopyTo(memoryStream);
        return memoryStream.ToArray();
    }
}
