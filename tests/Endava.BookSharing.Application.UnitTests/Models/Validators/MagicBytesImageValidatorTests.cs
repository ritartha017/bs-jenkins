using Endava.BookSharing.Application.Models.Validators;
using Endava.BookSharing.Application.UnitTests.Shared;
using Endava.BookSharing.Domain.Abstractions;
using Moq;
using Xunit;

namespace Endava.BookSharing.Application.UnitTests.Models.Validators
{
    public class MagicBytesImageValidatorTests
    {
        [Theory]
        [AutoMoqData]
        public void IsValidImage_ValidJpeg_ShouldReturnTrue(
            Mock<IFileData> file,
            MagicBytesImageValidator sut)
        {
            byte[] data = new byte[] { 0xFF, 0xD8, 0xFF, 0xE0, 0x00, 0x10, 0x4A, 0x46, 0x49, 0x46, 0x00, 0x01, 0x01, 0x00, 0x00, 0x01, 0x00, 0x01, 0x00, 0x00, 0xFF, 0xFE, 0x00, 0x3B, 0x43, 0x52, 0x45, 0x41, 0x54, 0x4F, 0x52, 0x3A };
            file.SetupGet(x => x.Raw).Returns(data);

            var result = sut.IsValidImage(file.Object);

            Assert.True(result);
        }

        [Theory]
        [AutoMoqData]
        public void IsValidImage_ValidJpg_ShouldReturnTrue(
            Mock<IFileData> file,
            MagicBytesImageValidator sut)
        {
            byte[] data = new byte[] { 0xFF, 0xD8, 0xFF, 0xE0, 0x55, 0x29, 0x4A, 0x46, 0x49, 0x46, 0x00, 0x01, 0x00, 0x00, 0x01, 0x00, 0x01, 0x00, 0x00, 0xFF, 0xFE, 0x00, 0x3B, 0x43, 0x52, 0x45, 0x41, 0x54, 0x4F, 0x52, 0x3A };
            file.SetupGet(x => x.Raw).Returns(data);

            var result = sut.IsValidImage(file.Object);

            Assert.True(result);
        }

        [Theory]
        [AutoMoqData]
        public void IsValidImage_ValidJfif_ShouldReturnTrue(
            Mock<IFileData> file,
            MagicBytesImageValidator sut)
        {
            byte[] data = new byte[] { 0xFF, 0xD8, 0xFF, 0xE1, 0x01, 0x00, 0x45, 0x78, 0x69, 0x66, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x01, 0x00, 0x00, 0xFF, 0xFE, 0x00, 0x3B, 0x43, 0x52, 0x45, 0x41, 0x54, 0x4F, 0x52, 0x3A };
            file.SetupGet(x => x.Raw).Returns(data);

            var result = sut.IsValidImage(file.Object);

            Assert.True(result);
        }

        [Theory]
        [AutoMoqData]
        public void IsValidImage_ValidPng_ShouldReturnTrue(
            Mock<IFileData> file,
            MagicBytesImageValidator sut)
        {
            byte[] data = new byte[] { 0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A, 0x00, 0x00, 0x00, 0x0D, 0x49, 0x48, 0x44, 0x52, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0x01, 0xCA, 0x08, 0x02, 0x00, 0x00, 0x00, 0xFA, 0x81, 0x64 };
            file.SetupGet(x => x.Raw).Returns(data);

            var result = sut.IsValidImage(file.Object);

            Assert.True(result);
        }

        [Theory]
        [AutoMoqData]
        public void IsValidImage_ValidBmp_ShouldReturnTrue(
            Mock<IFileData> file,
            MagicBytesImageValidator sut)
        {
            byte[] data = new byte[] { 0x42, 0x4D, 0x36, 0x98, 0x03, 0x00, 0x00, 0x00, 0x00, 0x00, 0x36, 0x04 };
            file.SetupGet(x => x.Raw).Returns(data);

            var result = sut.IsValidImage(file.Object);

            Assert.True(result);
        }

        [Theory]
        [AutoMoqData]
        public void IsValidImage_InvalidImage_ShouldReturnFalse(
            Mock<IFileData> file,
            MagicBytesImageValidator sut)
        {
            byte[] data = new byte[] { 0x4D, 0x5A, 0x90, 0x00, 0x03, 0x00, 0x00, 0x00, 0x04, 0x00 };
            file.SetupGet(x => x.Raw).Returns(data);

            var result = sut.IsValidImage(file.Object);

            Assert.False(result);
        }
    }
}
