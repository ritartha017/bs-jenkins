using System.Threading;
using System.Threading.Tasks;
using AutoFixture.Xunit2;
using Endava.BookSharing.Application.UserManagement.PasswordReset;
using Endava.BookSharing.Application.Models.Response;
using Endava.BookSharing.Presentation.Controllers;
using Endava.BookSharing.Presentation.UnitTests.Shared;
using Microsoft.AspNetCore.Http;
using MediatR;
using Moq;
using Xunit;
using System.Security.Claims;
using System.Collections.Generic;
using Endava.BookSharing.Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace Endava.BookSharing.Presentation.UnitTests.Controllers;

public class AccountControllerTests
{
    [Theory]
    [AutoMoqData]
    public void Logout_ShouldRemoveTokenCookie(
       [Greedy] AccountController sut,
       Mock<HttpResponse> httpResponseMock,
       Mock<HttpContext> httpContextMock,
       Mock<IResponseCookies> responseCookiesMock)
    {
        httpResponseMock.SetupGet(x => x.Cookies)
            .Returns(responseCookiesMock.Object);
        httpContextMock.SetupGet(x => x.Response)
            .Returns(httpResponseMock.Object);

        sut.ControllerContext = new ControllerContext
        {
            HttpContext = httpContextMock.Object
        };

        sut.Logout();

        responseCookiesMock.Verify(x => x.Delete("token"), Times.Once);
    }

    [Theory]
    [AutoMoqData]
    public async Task PasswordRequest_Mediator_CalledOnce(
        string email,
        ResetToken mediatorResponse,
        [Frozen] Mock<IMediator> mediatorMock,
        [Greedy] AccountController sut)
    {
        mediatorMock.Setup(x => x.Send(It.IsAny<ResetPasswordCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(mediatorResponse);

        var result = await sut.PasswordRequest(email, CancellationToken.None);

        mediatorMock.Verify(x => x.Send(It.Is<ResetPasswordCommand>(y => y.Email == email), It.IsAny<CancellationToken>()), Times.Once);
        Assert.Equal(mediatorResponse, result);
    }

    [Theory]
    [AutoMoqData]
    public async Task GetUserDetails_ShouldReturn_OkObjectResult(
        Mock<HttpContext> httpContextMock,
        [Greedy] AccountController sut)
    {
        sut.ControllerContext = new ControllerContext { HttpContext = httpContextMock.Object };
        var claims = new List<Claim>()
        {
            new Claim(Consts.UserIdClaimName, "user"),
            new Claim(Consts.RolesClaimName, string.Join(",", new Roles[] { Roles.User}))
        };
        var identity = new ClaimsIdentity(claims, "TestAuthType");
        httpContextMock.Object.User.AddIdentity(identity);

        var result = await sut.Account(CancellationToken.None);

        Assert.IsType<OkObjectResult>(result);
        Assert.NotNull(result);
    }
}