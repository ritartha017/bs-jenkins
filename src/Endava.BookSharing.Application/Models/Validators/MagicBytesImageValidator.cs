using Endava.BookSharing.Application.Abstract;
using Endava.BookSharing.Domain.Abstractions;

namespace Endava.BookSharing.Application.Models.Validators
{
    public class MagicBytesImageValidator : IImageValidator
    {
        private static readonly byte?[] JPG = new byte?[] { 0xFF, 0xD8, 0xFF, 0xDB };
        private static readonly byte?[] JPEG = new byte?[] { 0xFF, 0xD8, 0xFF, 0xE0, null, null, 0x4A, 0x46, 0x49, 0x46, 0x00, 0x01 };
        private static readonly byte?[] JFIF = new byte?[] { 0xFF, 0xD8, 0xFF, 0xE1, null, null, 0x45, 0x78, 0x69, 0x66, 0x00, 0x00 };
        private static readonly byte?[] PNG = new byte?[] { 0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A };
        private static readonly byte?[] BMP = new byte?[] { 0x42, 0x4D };

        private static readonly byte?[][] AllowedSignatures = new byte?[][]
        {
            JPG, JPEG, JFIF, PNG, BMP
        };

        public bool IsValidImage(IFileData file)
        {
            return AllowedSignatures.Any(x => MatchesSignature(x, file.Raw));
        }

        private bool MatchesSignature(byte?[] signature, byte[] actual)
        {
            if (actual.Length < signature.Length) return false;

            for (int i = 0; i < signature.Length; i++)
            {
                if (signature[i] is null) continue;

                if (actual[i] != signature[i]) return false;
            }

            return true;
        }
    }
}
