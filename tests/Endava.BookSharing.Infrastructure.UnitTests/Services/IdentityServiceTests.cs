#pragma warning disable CS8620 // Argument cannot be used for parameter due to differences in the nullability of reference types.

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using Endava.BookSharing.Application.Abstract;
using Endava.BookSharing.Application.Exceptions;
using Endava.BookSharing.Application.Models;
using Endava.BookSharing.Infrastructure.Persistence.Identity.Models;
using Endava.BookSharing.Infrastructure.Services;
using Endava.BookSharing.Infrastructure.UnitTests.Shared;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Moq;
using Xunit;

namespace Endava.BookSharing.Infrastructure.UnitTests.Services
{
    public class IdentityServiceTests
    {
        [Theory]
        [AutoMoqData]
        public async Task CreatePasswordResetToken_UserNotFound_ShouldThrowException(string email)
        {
            var sut = CreateSUT(out _, out _, out _, out Mock<UserManager<UserDb>> userManagerMock);
            userManagerMock.Setup(x => x.FindByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync((UserDb?)null);

            await Assert.ThrowsAsync<BookSharingGenericException>(() => sut.CreatePasswordResetToken(email, CancellationToken.None));
        }

        [Theory]
        [AutoMoqData]
        public async Task ValidateUserCredentials_WithNotExistendUserName_ShouldReturnNull(
            string username, string password)
        {
            var sut = CreateSUT(out _, out Mock<IUserRepository> userRepositoryMock, out _, out _);
            userRepositoryMock.Setup(static repository =>
               repository.ExistsUserNameAsync(It.IsAny<string>(),
                   It.IsAny<CancellationToken>()))
           .ReturnsAsync(false);

            var result = await sut.ValidateUserCredentialsAsync(username, password, CancellationToken.None);

            Assert.Null(result);
            //result.Should().BeNull();
        }

        [Theory]
        [AutoMoqData]
        public async Task CreatePasswordResetToken_UserFound_ShouldReturnGeneratedPasswordResetToken(UserDb user, string email, string token)
        {
            var sut = CreateSUT(out _, out _, out _, out Mock<UserManager<UserDb>> userManagerMock);
            userManagerMock.Setup(x => x.FindByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(user);

            userManagerMock.Setup(x => x.GeneratePasswordResetTokenAsync(It.IsAny<UserDb>()))
                .ReturnsAsync(token);

            var result = await sut.CreatePasswordResetToken(email, CancellationToken.None);

            userManagerMock.Verify(x => x.GeneratePasswordResetTokenAsync(user), Times.Once);
            Assert.Equal(token, result);
        }

        [Theory]
        [AutoMoqData]
        public async Task ValidateUserCredentials_WithExistingUserNameAndInvalidPassword_ShouldReturnNull(
            string username, string password, UserDb user)
        {
            var sut = CreateSUT(out _, out Mock<IUserRepository> userRepositoryMock, out Mock<SignInManager<UserDb>> signInManagerMock,
                out Mock<UserManager<UserDb>> userManagerMock);

            userRepositoryMock.Setup(static repository =>
               repository.ExistsUserNameAsync(It.IsAny<string>(),
                   It.IsAny<CancellationToken>()))
           .ReturnsAsync(true);

            userManagerMock.Setup(manager => manager.FindByNameAsync(It.IsAny<string>()))
                .ReturnsAsync(user);

            signInManagerMock.Setup(static repository =>
                repository.CheckPasswordSignInAsync(It.IsAny<UserDb>(), It.IsAny<string>(),
                    false)).ReturnsAsync(SignInResult.NotAllowed);

            var result = await sut.ValidateUserCredentialsAsync(username, password, CancellationToken.None);
            result.Should().BeNull();
        }

        private static IdentityService CreateSUT(
            out Mock<IRoleRepository> roleRepositorMock,
            out Mock<IUserRepository> userRepositoryMock,
            out Mock<SignInManager<UserDb>> signinManagerMock,
            out Mock<UserManager<UserDb>> userManagerMock)
        {
            var userStoreMock = new Mock<IUserStore<UserDb>>();
            userManagerMock = new Mock<UserManager<UserDb>>(userStoreMock.Object,
                null, null, null, null, null, null, null, null);

            var contextAccessor = new Mock<IHttpContextAccessor>();
            var userPrincipalFactory = new Mock<IUserClaimsPrincipalFactory<UserDb>>();

            signinManagerMock = new Mock<SignInManager<UserDb>>(userManagerMock.Object,
                contextAccessor.Object, userPrincipalFactory.Object, null, null, null, null);

            roleRepositorMock = new Mock<IRoleRepository>();
            userRepositoryMock = new Mock<IUserRepository>();

            return new IdentityService(roleRepositorMock.Object, userRepositoryMock.Object, signinManagerMock.Object, userManagerMock.Object);
        }

        [Theory]
        [AutoMoqData]
        public async Task ValidateUserCredentials_WithExistingUserNameAndValidPassword_ShouldRecieveUserRoles(
            string username, string password, UserDb user, IFixture fixture)
        {
            var sut = CreateSUT(out _, out Mock<IUserRepository> userRepositoryMock, out Mock<SignInManager<UserDb>> signInManagerMock,
                out Mock<UserManager<UserDb>> userManagerMock);

            userRepositoryMock.Setup(static repository =>
               repository.ExistsUserNameAsync(It.IsAny<string>(),
                   It.IsAny<CancellationToken>()))
           .ReturnsAsync(true);

            userManagerMock.Setup(static manager => manager.FindByNameAsync(It.IsAny<string>())).ReturnsAsync(fixture.Create<UserDb>());

            signInManagerMock.Setup(static repository =>
                repository.CheckPasswordSignInAsync(It.IsAny<UserDb>(), It.IsAny<string>(),
                    false)).ReturnsAsync(SignInResult.Success);

            await sut.ValidateUserCredentialsAsync(username, password, CancellationToken.None);

            userManagerMock.Verify(static r => r.GetRolesAsync(It.IsAny<UserDb>()), Times.Once);
        }

        [Theory]
        [AutoMoqData]
        public async Task ValidateUserCredentials_WithExistingUserNameAndValidPasswordAndExistingRoles_ShouldReturnUserRolesDtoType(
            string username, string password, UserDb user, List<string> roles)
        {
            var sut = CreateSUT(out _, out Mock<IUserRepository> userRepositoryMock, out Mock<SignInManager<UserDb>> signInManagerMock,
                out Mock<UserManager<UserDb>> userManagerMock);

            userRepositoryMock.Setup(static repository =>
               repository.ExistsUserNameAsync(It.IsAny<string>(),
                   It.IsAny<CancellationToken>()))
           .ReturnsAsync(true);

            userManagerMock.Setup(static manager => manager.FindByNameAsync(It.IsAny<string>())).ReturnsAsync(user);

            signInManagerMock.Setup(static repository =>
                repository.CheckPasswordSignInAsync(It.IsAny<UserDb>(), It.IsAny<string>(),
                    false)).ReturnsAsync(SignInResult.Success);

            userManagerMock.Setup(static r => r.GetRolesAsync(It.IsAny<UserDb>())).ReturnsAsync(roles);

            var result = await sut.ValidateUserCredentialsAsync(username, password, CancellationToken.None);
            Assert.NotNull(result);
            Assert.Equal(user.Id, result!.UserId);
            Assert.Equal(roles, result!.UserRoles);
            result.Should().BeOfType<UserRolesDto?>();
        }
    }
}

#pragma warning restore CS8620 // Argument cannot be used for parameter due to differences in the nullability of reference types.