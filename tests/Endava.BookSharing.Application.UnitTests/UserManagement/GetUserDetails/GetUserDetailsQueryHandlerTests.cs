using AutoFixture.Xunit2;
using Endava.BookSharing.Application.Abstract;
using Endava.BookSharing.Application.Exceptions;
using Endava.BookSharing.Application.Models;
using Endava.BookSharing.Application.UnitTests.Shared;
using Endava.BookSharing.Application.UserManagement.GetUserDetails;
using Endava.BookSharing.Domain.Entities;
using Endava.BookSharing.Domain.Enums;
using Moq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Endava.BookSharing.Application.UnitTests.UserManagement.GetUserDetails;

public class GetUserDetailsQueryHandlerTests
{
    [Theory]
    [AutoMoqData]
    public async Task Handle_ReturnCorrectUserDetails(
        [Frozen] Mock<IUserRepository> userRepository,
        GetUserDetailsQueryHandler sut,
        User user)
    {
        var authenticatedUser = new AuthenticatedUser("id", new Roles[] { Roles.User });
        user.Id = authenticatedUser.Id;
        userRepository.Setup(x => x.GetByIdAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        var query = new GetUserDetailsQuery(authenticatedUser);
        var result = await sut.Handle(query, CancellationToken.None);

        Assert.Equal(user.Id, result.Id);
        Assert.Equal(user.FirstName, result.FirstName);
        Assert.Equal(user.LastName, result.LastName);
        Assert.Equal(user.UserName, result.UserName);
        Assert.Equal(user.Email, result.Email);
        Assert.Equal("User", result.Role);
    }

    [Theory]
    [AutoMoqData]
    public async Task Handle_WhenInvalidUserId_ThrowNotFoundException(
       [Frozen] Mock<IUserRepository> userRepository,
       GetUserDetailsQueryHandler sut,
       AuthenticatedUser authenticatedUser)
    {
        userRepository.Setup(x => x.GetByIdAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
           .ReturnsAsync((User)null!);

        var query = new GetUserDetailsQuery(authenticatedUser);

        await Assert.ThrowsAsync<BookSharingNotFoundException>(async () 
            => await sut.Handle(query, CancellationToken.None));
    }
}