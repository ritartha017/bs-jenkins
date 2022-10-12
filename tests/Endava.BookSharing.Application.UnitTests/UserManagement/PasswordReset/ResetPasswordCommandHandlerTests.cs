using System.Threading;
using System.Threading.Tasks;
using AutoFixture.Xunit2;
using Endava.BookSharing.Application.Abstract;
using Endava.BookSharing.Application.UserManagement.PasswordReset;
using Endava.BookSharing.Application.UnitTests.Shared;
using Moq;
using Xunit;

namespace Endava.BookSharing.Application.UnitTests.UserManagement.PasswordReset;

public class ResetPasswordCommandHandlerTests
{
    [Theory]
    [AutoMoqData]
    public async Task Handle_ShouldCallCreatePasswordResetToken(
        [Frozen] Mock<IIdentityService> identityServiceMock,
        string email,
        ResetPasswordCommandHandler sut)
    {
        var command = new ResetPasswordCommand(email);

        await sut.Handle(command, CancellationToken.None);

        identityServiceMock.Verify(x => x.CreatePasswordResetToken(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once);
        identityServiceMock.Verify(x => x.CreatePasswordResetToken(command.Email, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Theory]
    [AutoMoqData]
    public async Task Handle_ShouldReturnCreatedPasswordResetToken(
        [Frozen] Mock<IIdentityService> identityServiceMock,
        string email,
        string token,
        ResetPasswordCommandHandler sut)
    {
        identityServiceMock.Setup(x => x.CreatePasswordResetToken(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(token);

        var command = new ResetPasswordCommand(email);

        var result = await sut.Handle(command, CancellationToken.None);

        Assert.NotNull(result);
        Assert.Equal(token, result.Hash);
    }
}