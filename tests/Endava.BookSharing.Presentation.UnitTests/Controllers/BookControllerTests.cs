using AutoFixture.Xunit2;
using Endava.BookSharing.Application.BookManagement.DeleteBook;
using Endava.BookSharing.Application.BookManagement.UpdateBook;
using Endava.BookSharing.Domain.Enums;
using Endava.BookSharing.Presentation.Controllers;
using Endava.BookSharing.Presentation.Models.Requests;
using Endava.BookSharing.Presentation.UnitTests.Shared;
using Endava.BookSharing.Presentation.UnitTests.Shared.Models;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Endava.BookSharing.Application.BookManagement.GetBook;
using Endava.BookSharing.Application.Models;
using Endava.BookSharing.Application.Models.Requests;
using Endava.BookSharing.Application.Models.Response;
using FluentAssertions;
using Xunit;
using Endava.BookSharing.Application.UserManagement.BookFeedback;

namespace Endava.BookSharing.Presentation.UnitTests.Controllers;

public class BookControllerTests
{
    [Theory]
    [AutoMoqData]
    public async Task BookController_Update_ShouldReturnOkCode200(
        [Greedy] BookController sut,
        Mock<HttpContext> httpContextMock,
        Mock<IMediator> mediator
        )
    {
        var expectedResult = sut.Ok();

        mediator.Setup(x => x.Send(It.IsAny<UpdateBookCommand>(), CancellationToken.None))
            .ReturnsAsync(Unit.Value);

        sut.ControllerContext = new ControllerContext
        {
            HttpContext = httpContextMock.Object
        };

        var claims = new List<Claim>()
        {
            new Claim(Consts.UserIdClaimName, "user"),
            new Claim(Consts.RolesClaimName, string.Join(",", new Roles[] { Roles.User}))
        };
        var identity = new ClaimsIdentity(claims, "TestAuthType");

        httpContextMock.Object.User.AddIdentity(identity);
        var request = new BookUpdateRequest()
        {
            Title = string.Empty,
            PublicationDate = string.Empty,
            Genres = new List<string>() { string.Empty },
            LanguageId = string.Empty,
            AuthorId = string.Empty,
            AuthorName = string.Empty,
            Cover = new FormData()
        };
        IActionResult actualResult = await sut.Update(string.Empty,
            request,
            CancellationToken.None);

        Assert.IsType<OkResult>(actualResult);
        Assert.Equal(expectedResult.StatusCode, ((OkResult)actualResult).StatusCode);
    }

    [Theory]
    [AutoMoqData]
    public async Task BookController_Delete_ShouldReturnOkCode200(
        [Greedy] BookController sut,
        Mock<HttpContext> httpContextMock,
        Mock<IMediator> mediator
        )
    {
        var expectedResult = sut.Ok();

        mediator.Setup(x => x.Send(It.IsAny<DeleteBookCommand>(), CancellationToken.None))
            .ReturnsAsync(Unit.Value);

        sut.ControllerContext = new ControllerContext
        {
            HttpContext = httpContextMock.Object
        };

        var claims = new List<Claim>()
        {
            new Claim(Consts.UserIdClaimName, "user"),
            new Claim(Consts.RolesClaimName, string.Join(",", new Roles[] { Roles.User}))
        };
        var identity = new ClaimsIdentity(claims, "TestAuthType");

        httpContextMock.Object.User.AddIdentity(identity);

        IActionResult actualResult = await sut.Delete(string.Empty,
            DeletionType.Full,
            CancellationToken.None);

        Assert.IsType<OkResult>(actualResult);
        Assert.Equal(expectedResult.StatusCode, ((OkResult)actualResult).StatusCode);
    }

    [Theory]
    [AutoMoqData]
    public async Task BookController_Delete_ShouldReturnBadRequest400WhenDeletionTypeInvalid(
        [Greedy] BookController sut,
        Mock<HttpContext> httpContextMock,
        Mock<IMediator> mediator
        )
    {
        var expectedStatusCode = (int)System.Net.HttpStatusCode.BadRequest;

        mediator.Setup(x => x.Send(It.IsAny<DeleteBookCommand>(), CancellationToken.None))
            .ReturnsAsync(Unit.Value);

        sut.ControllerContext = new ControllerContext
        {
            HttpContext = httpContextMock.Object
        };

        var claims = new List<Claim>()
        {
            new Claim(Consts.UserIdClaimName, "user"),
            new Claim(Consts.RolesClaimName, string.Join(",", new Roles[] { Roles.User}))
        };
        var identity = new ClaimsIdentity(claims, "TestAuthType");

        httpContextMock.Object.User.AddIdentity(identity);

        IActionResult actualResult = await sut.Delete(string.Empty,
            DeletionType.None,
            CancellationToken.None);

        Assert.IsType<BadRequestObjectResult>(actualResult);
        Assert.Equal(expectedStatusCode, ((BadRequestObjectResult)actualResult).StatusCode);
    }

    [Theory]
    [AutoMoqData]
    public async Task GetBookDetails_Mediator_CalledOnce(
        string bookId,
        BookResponse mediatorResponse,
        [Frozen] Mock<IMediator> mediatorMock,
        [Frozen] Mock<HttpContext> httpContextMock,
        [Greedy] BookController sut)
    {
        mediatorMock.Setup(x => x.Send(It.IsAny<GetBookCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(mediatorResponse);

        sut.ControllerContext = new ControllerContext
        {
            HttpContext = httpContextMock.Object
        };
        var claims = new List<Claim>()
        {
            new Claim(Consts.UserIdClaimName, "user"),
            new Claim(Consts.RolesClaimName, string.Join(",", new Roles[] { Roles.User}))
        };
        var identity = new ClaimsIdentity(claims, "TestAuthType");

        httpContextMock.Object.User.AddIdentity(identity);

        var result = await sut.GetBookDetails(bookId, CancellationToken.None);

        mediatorMock.Verify(x => x.Send(It.Is<GetBookCommand>(y => y.BookId == bookId), It.IsAny<CancellationToken>()), Times.Once);
        result.Should().BeEquivalentTo(mediatorResponse, options
            => options.ComparingByMembers<GetBookCommand>().ExcludingMissingMembers());
    }

    [Theory]
    [AutoMoqData]
    public async Task CreateReview_Mediator_CalledOnce(
        [Frozen] Mock<IMediator> mediatorMock,
        [Frozen] Mock<HttpContext> httpContextMock,
        [Greedy] BookController sut,
        CreateReviewRequest request)
    {
        sut.ControllerContext = new ControllerContext
        {
            HttpContext = httpContextMock.Object
        };
        var claims = new List<Claim>()
        {
            new Claim(Consts.UserIdClaimName, "user"),
            new Claim(Consts.RolesClaimName, string.Join(",", new Roles[] { Roles.User}))
        };
        var identity = new ClaimsIdentity(claims, "TestAuthType");

        httpContextMock.Object.User.AddIdentity(identity);

        await sut.CreateReviewAsync(request, CancellationToken.None);

        mediatorMock.Verify(mediator => mediator.Send(It.IsAny<CreateReviewCommand>(),
            It.IsAny<CancellationToken>()), Times.Once);
    }

    [Theory]
    [AutoMoqData]
    public async Task CreateReview_ReturnStatusCode200(
        [Frozen] Mock<IMediator> mediatorMock,
        [Frozen] Mock<HttpContext> httpContextMock,
        [Greedy] BookController sut,
        CreateReviewRequest request)
    {
        var statusCode200 = sut.Ok();
        sut.ControllerContext = new ControllerContext
        {
            HttpContext = httpContextMock.Object
        };
        var claims = new List<Claim>()
        {
            new Claim(Consts.UserIdClaimName, "user"),
            new Claim(Consts.RolesClaimName, string.Join(",", new Roles[] { Roles.User}))
        };
        var identity = new ClaimsIdentity(claims, "TestAuthType");

        httpContextMock.Object.User.AddIdentity(identity);

        mediatorMock.Setup(mediator => mediator.Send(It.IsAny<CreateReviewCommand>(),
            It.IsAny<CancellationToken>())).ReturnsAsync(Unit.Value);

        var result = await sut.CreateReviewAsync(request, CancellationToken.None);
        result.Should().BeEquivalentTo(statusCode200);
    }
}