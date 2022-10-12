using Endava.BookSharing.Domain.Abstractions;
using MediatR;

namespace Endava.BookSharing.Application.FileManagement.CreateFile;

public class CreateFileCommand : IRequest<string>, IFileData
{
    public CreateFileCommand(string contentType, byte[] raw)
    {
        ContentType = contentType;
        Raw = raw;
    }
    public string ContentType { get; set; }
    public byte[] Raw { get; set; }
}
