using System;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.Xunit2;
using Endava.BookSharing.Application.Abstract;
using Endava.BookSharing.Application.Exceptions;
using Endava.BookSharing.Application.UnitTests.Shared;
using Endava.BookSharing.Application.UserManagement.UserSignUp;
using Endava.BookSharing.Domain.Entities;
using Endava.BookSharing.Domain.Enums;
using FluentAssertions;
using Moq;
using Xunit;

namespace Endava.BookSharing.Application.UnitTests.UserManagement.SignUpUser;

public class UserSignUpCommandHandlerTests
{
    [Theory]
    [AutoMoqData]
    public async Task Handle_WithUniqueEmailAndUserName_UserCreationFailed_ShouldThrowException(
        UserSignUpCommand commandStub,
        [Frozen] Mock<IUserRepository> userRepositoryMock,
        UserSignUpCommandHandler sut)
    {
        userRepositoryMock.Setup(static repository =>
                repository.ExistsUserNameAsync(It.IsAny<string>(),
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);
        userRepositoryMock.Setup(static repository =>
                repository.ExistsEmailAsync(It.IsAny<string>(),
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);
        userRepositoryMock.Setup(static repository =>
                repository.CreateAsync(It.IsAny<User>(), It.IsAny<string>(),
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        await Assert.ThrowsAsync<BookSharingGenericException>(() =>
            sut.Handle(commandStub, CancellationToken.None));

        userRepositoryMock.Verify(static r =>
            r.CreateAsync(It.IsAny<User>(), It.IsAny<string>(),
                It.IsAny<CancellationToken>()), Times.Once);
    }

    public async Task Handle_WithNonUniqueEmail_UserCreationFailed_ShouldThrowException(
        UserSignUpCommand commandStub,
        [Frozen] Mock<IUserRepository> userRepositoryMock,
        UserSignUpCommandHandler sut)
    {
        userRepositoryMock.Setup(static repository =>
                repository.ExistsEmailAsync(It.IsAny<string>(),
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        await Assert.ThrowsAsync<BookSharingGenericException>(() =>
            sut.Handle(commandStub, CancellationToken.None));

        userRepositoryMock.Verify(static r =>
            r.CreateAsync(It.IsAny<User>(), It.IsAny<string>(),
                It.IsAny<CancellationToken>()), Times.Never);
    }

    public async Task Handle_WithNonUniqueUserName_UserCreationFailed_ShouldThrowException(
        UserSignUpCommand commandStub,
        [Frozen] Mock<IUserRepository> userRepositoryMock,
        UserSignUpCommandHandler sut)
    {
        userRepositoryMock.Setup(static repository =>
                repository.ExistsEmailAsync(It.IsAny<string>(),
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);
        userRepositoryMock.Setup(static repository =>
                repository.ExistsUserNameAsync(It.IsAny<string>(),
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        await Assert.ThrowsAsync<BookSharingGenericException>(() =>
            sut.Handle(commandStub, CancellationToken.None));

        userRepositoryMock.Verify(static r =>
            r.CreateAsync(It.IsAny<User>(), It.IsAny<string>(),
                It.IsAny<CancellationToken>()), Times.Never);
    }

    [Theory]
    [AutoMoqData]
    public async Task Handle_WithUniqueEmailAndUserName_UserCreated_ShouldEnsureRoleExists(
        UserSignUpCommand commandStub,
        [Frozen] Mock<IUserRepository> userRepositoryMock,
        [Frozen] Mock<IIdentityService> identityServiceMock,
        UserSignUpCommandHandler sut)
    {
        userRepositoryMock.Setup(static repository =>
                repository.ExistsUserNameAsync(It.IsAny<string>(),
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);
        userRepositoryMock.Setup(static repository =>
                repository.ExistsEmailAsync(It.IsAny<string>(),
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);
        userRepositoryMock.Setup(static repository =>
                repository.CreateAsync(It.IsAny<User>(), It.IsAny<string>(),
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);
        identityServiceMock.Setup(static service =>
            service.AddUserToRole(It.IsAny<User>(), It.IsAny<Roles>(),
                It.IsAny<CancellationToken>())).ReturnsAsync(true);

        await sut.Handle(commandStub, CancellationToken.None);

        identityServiceMock.Verify(t => t.CreateRoleIfNotExists
            (It.Is<Roles>(s => s == Roles.User), It.IsAny<CancellationToken>()), Times.Once);

        userRepositoryMock.Verify(static r =>
            r.CreateAsync(It.IsAny<User>(), It.IsAny<string>(),
                It.IsAny<CancellationToken>()), Times.Once);
    }

    [Theory]
    [AutoMoqData]
    public async Task Handle_WithUniqueEmailAndUserName_UserCreated_AssignRoleFailed_ShouldThrowException(
        UserSignUpCommand command,
        [Frozen] Mock<IUserRepository> userRepositoryMock,
        [Frozen] Mock<IIdentityService> identityServiceMock,
        UserSignUpCommandHandler sut)
    {
        userRepositoryMock.Setup(static repository =>
                repository.ExistsUserNameAsync(It.IsAny<string>(),
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);
        userRepositoryMock.Setup(static repository =>
                repository.ExistsEmailAsync(It.IsAny<string>(),
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);
        userRepositoryMock.Setup(static repository =>
                repository.CreateAsync(It.IsAny<User>(), It.IsAny<string>(),
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);
        identityServiceMock.Setup(static service =>
            service.AddUserToRole(It.IsAny<User>(), It.IsAny<Roles>(),
                It.IsAny<CancellationToken>())).ReturnsAsync(false);

        Task Act()
        {
            return sut.Handle(command, CancellationToken.None);
        }

        await Assert.ThrowsAsync<BookSharingGenericException>(Act);

        identityServiceMock.Verify(static r => r.AddUserToRole
        (It.IsAny<User>(), It.Is<Roles>(s => s == Roles.User),
                It.IsAny<CancellationToken>()), Times.Once);
    }

    [Theory]
    [AutoMoqData]
    public async Task Handle_WithUniqueEmailAndUserName_UserCreated_AssignRoleSuccess_ShouldReturnUnit
    (
        UserSignUpCommand command,
        [Frozen] Mock<IIdentityService> identityServiceMock,
        [Frozen] Mock<IUserRepository> userRepositoryMock,
        UserSignUpCommandHandler sut)
    {
        userRepositoryMock.Setup(static repository =>
                repository.ExistsUserNameAsync(It.IsAny<string>(),
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);
        userRepositoryMock.Setup(static repository =>
                repository.ExistsEmailAsync(It.IsAny<string>(),
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);
        identityServiceMock.Setup(static service =>
            service.AddUserToRole(It.IsAny<User>(), It.IsAny<Roles>(),
                It.IsAny<CancellationToken>())).ReturnsAsync(true);

        userRepositoryMock.Setup(static repository =>
                repository.CreateAsync(It.IsAny<User>(), It.IsAny<string>(),
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        Func<Task> act = async () =>
        {
            await sut.Handle(command, CancellationToken.None);
        };

        await act.Should().NotThrowAsync();
    }
}