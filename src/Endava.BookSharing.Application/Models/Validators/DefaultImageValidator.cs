using Endava.BookSharing.Application.Abstract;
using Endava.BookSharing.Domain.Abstractions;
using System.Drawing;

namespace Endava.BookSharing.Application.Models.Validators;

public class DefaultImageValidator : IImageValidator
{
    public bool IsValidImage(IFileData file)
    {
        Image? parsedImage = null;
        try
        {
            using var stream = new MemoryStream(file.Raw);
            parsedImage = Image.FromStream(stream);

            return parsedImage is not null;
        }
        catch (Exception)
        {
            return false;
        }
        finally
        {
            parsedImage?.Dispose();
        }
    }
}
