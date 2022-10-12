using AutoFixture.Xunit2;
using Endava.BookSharing.Application.Abstract;
using Endava.BookSharing.Application.UserManagement.ResetPassword;
using Endava.BookSharing.Application.Exceptions;
using Endava.BookSharing.Application.UnitTests.Shared;
using Moq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Endava.BookSharing.Application.UnitTests.UserManagement.ResetPassword;

public class UserVerifyResetPasswordHashCommandHandlerTests
{
    [Theory]
    [AutoMoqData]
    public async Task Hadle_VerifyPasswordResetToken_ReturnsTrue(
        UserVerifyResetPasswordHashCommand command,
        [Frozen] Mock<IIdentityService> identityServiceMock,
        UserVerifyResetPasswordHashCommandHandler handler)
    {
        identityServiceMock.Setup(static s =>
            s.VerifyResetPasswordHash(
                It.IsAny<string>(), 
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        await handler.Handle(command, CancellationToken.None);

        identityServiceMock.Verify(static s =>
            s.VerifyResetPasswordHash(
                It.IsAny<string>(), 
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()), Times.Once);
    }

    [Theory]
    [AutoMoqData]
    public async Task Hadle_VerifyPasswordResetTokenReturnFalse_ShouldThowException(
        UserVerifyResetPasswordHashCommand command,
        [Frozen] Mock<IIdentityService> identityServiceMock,
        UserVerifyResetPasswordHashCommandHandler handler)
    {
        identityServiceMock.Setup(static s =>
            s.VerifyResetPasswordHash(
                It.IsAny<string>(), 
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        await Assert.ThrowsAsync<BookSharingGenericException>(() =>
            handler.Handle(command, CancellationToken.None));

        identityServiceMock.Verify(static s =>
            s.VerifyResetPasswordHash(
                It.IsAny<string>(), 
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()), Times.Once);

    }
}

