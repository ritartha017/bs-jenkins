using Endava.BookSharing.Domain.Abstractions;

namespace Endava.BookSharing.Application.Abstract;

public interface IImageValidator
{
    bool IsValidImage(IFileData file);
}
