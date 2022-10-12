using System.Threading;
using System.Threading.Tasks;
using AutoFixture.Xunit2;
using Endava.BookSharing.Application.Abstract;
using Endava.BookSharing.Application.Exceptions;
using Endava.BookSharing.Application.UnitTests.Shared;
using Endava.BookSharing.Application.UserManagement.AssignRole;
using Endava.BookSharing.Domain.Enums;
using MediatR;
using Moq;
using Xunit;

namespace Endava.BookSharing.Application.UnitTests.UserManagement.AssignRole;

public class AssignRoleCommandHandlerTests
{
    [Theory]
    [AutoMoqData]
    public async Task Handle_ShouldCallMethodsWithValidParameters(
        [Frozen] Mock<IIdentityService> identityServiceMock,
        string id,
        Roles role,
        AssignRoleCommandHandler sut)
    {
        identityServiceMock.Setup(x => x.IsRoleAssigned(It.IsAny<string>(),
                It.IsAny<Roles>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);
        identityServiceMock.Setup(x => x.UpdateRole(It.IsAny<string>(),
                It.IsAny<Roles>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);
        var command = new AssignRoleCommand(id, role);

        await sut.Handle(command, CancellationToken.None);
        
        identityServiceMock.Verify(x => x.IsRoleAssigned(command.Id, Roles.User,
            It.IsAny<CancellationToken>()), Times.Once);
        identityServiceMock.Verify(x => x.UpdateRole(command.Id, Roles.User,
            It.IsAny<CancellationToken>()), Times.Once);
    }

    [Theory]
    [AutoMoqData]
    public async Task Handle_ShouldReturnUnit(
        [Frozen] Mock<IIdentityService> identityServiceMock,
        string id,
        Roles role,
        AssignRoleCommandHandler sut)
    {
        identityServiceMock.Setup(x => x.IsRoleAssigned(It.IsAny<string>(),
                It.IsAny<Roles>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);
        identityServiceMock.Setup(x => x.UpdateRole(It.IsAny<string>(),
                It.IsAny<Roles>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);
        var command = new AssignRoleCommand(id, role);

        var result = await sut.Handle(command, CancellationToken.None);

        Assert.IsType<Unit>(result);
    }

    [Theory]
    [AutoMoqData]
    public async Task Handle_WithNotValidId_ShouldThrow(
        [Frozen] Mock<IIdentityService> identityServiceMock,
        string id,
        Roles role,
        AssignRoleCommandHandler sut)
    {
        identityServiceMock.Setup(x => x.IsRoleAssigned(It.IsAny<string>(),
                It.IsAny<Roles>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);
        identityServiceMock.Setup(x => x.UpdateRole(It.IsAny<string>(),
                It.IsAny<Roles>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);
        var command = new AssignRoleCommand(id, role);

        await Assert.ThrowsAsync<BookSharingEntityAlreadyExistException>(() =>
            sut.Handle(command, CancellationToken.None));
        identityServiceMock.Verify(x => x.IsRoleAssigned(command.Id, Roles.User,
            It.IsAny<CancellationToken>()), Times.Once);
        identityServiceMock.Verify(x => x.UpdateRole(command.Id, Roles.User,
            It.IsAny<CancellationToken>()), Times.Never);
    }
    
    [Theory]
    [AutoMoqData]
    public async Task Handle_WithNotRole_ShouldThrow(
        [Frozen] Mock<IIdentityService> identityServiceMock,
        string id,
        Roles role,
        AssignRoleCommandHandler sut)
    {
        identityServiceMock.Setup(x => x.IsRoleAssigned(It.IsAny<string>(),
                It.IsAny<Roles>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);
        identityServiceMock.Setup(x => x.UpdateRole(It.IsAny<string>(),
                It.IsAny<Roles>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);
        var command = new AssignRoleCommand(id, role);

        await Assert.ThrowsAsync<BookSharingInternalException>(() =>
            sut.Handle(command, CancellationToken.None));
        identityServiceMock.Verify(x => x.IsRoleAssigned(command.Id, Roles.User,
            It.IsAny<CancellationToken>()), Times.Once);
        identityServiceMock.Verify(x => x.UpdateRole(command.Id, Roles.User,
            It.IsAny<CancellationToken>()), Times.Once);
    }
}