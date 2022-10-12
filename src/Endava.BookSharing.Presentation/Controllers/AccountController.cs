using Endava.BookSharing.Application.Exceptions;
using Endava.BookSharing.Application.Models.Requests;
using Endava.BookSharing.Application.UserManagement.ResetPassword;
using Endava.BookSharing.Application.UserManagement.UserSignIn;
using Endava.BookSharing.Application.UserManagement.PasswordReset;
using Endava.BookSharing.Presentation.Helpers;
using Endava.BookSharing.Application.Models.Response;
using Endava.BookSharing.Application.UserManagement.GetUserDetails;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Endava.BookSharing.Application.UserManagement.UserSignUp;
using Endava.BookSharing.Presentation.Extensions;
using Microsoft.AspNetCore.Authorization;

namespace Endava.BookSharing.Presentation.Controllers;

[Route("[controller]")]
[ApiController]
public class AccountController : ControllerBase
{
    private readonly IMediator mediator;
    private readonly JwtTokenHelper jwtTokenHelper;

    public AccountController(IMediator mediator, JwtTokenHelper jwtTokenHelper)
    {
        this.mediator = mediator;
        this.jwtTokenHelper = jwtTokenHelper;
    }

    [HttpPost("register")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
    public async Task<IActionResult> SignUp([FromBody] UserSignUpRequest request, CancellationToken cancellationToken)
    {
        var command = new UserSignUpCommand(request);
        await mediator.Send(command, cancellationToken);

        return Ok();
    }

    [HttpPost("logout")]
    public IActionResult Logout()
    {
        Response.Cookies.Delete(Consts.TokenCookieName);
        return Ok();
    }

    [HttpPost("login")]
    public async Task<IActionResult> LoginAsync(UserSignInRequest request, CancellationToken cancellationToken)
    {
        var command = new CheckCredentialsQuery(request);
        var checkedUser = await mediator.Send(command, cancellationToken);
        if (checkedUser is null) throw new BookSharingInvalidCredentialsException();

        var jwtToken = jwtTokenHelper.CreateAuthToken(checkedUser.UserId, checkedUser.UserRoles);
        Response.Cookies.Append(Consts.TokenCookieName, jwtToken, new CookieOptions()
        {
            HttpOnly = true,
            Expires = DateTimeOffset.MaxValue
        });
        return Ok();
    }

    [HttpPost("reset")]
    public async Task<IActionResult> ResetPassword([FromBody] UserResetPasswordRequest request, CancellationToken cancellationToken)
    {
        var command = new UserResetPasswordCommand(request);
        await mediator.Send(command, cancellationToken);

        return Ok();
    }

    [HttpPost("reset/verify")]
    public async Task<IActionResult> VerifyResetPasswordHash([FromBody] UserVerifyResetPasswordHashRequest request, CancellationToken cancellationToken)
    {
        var command = new UserVerifyResetPasswordHashCommand(request);
        await mediator.Send(command, cancellationToken);

        return Ok();
    }

    [HttpPost("reset/start")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
    public async Task<ResetToken> PasswordRequest(string email, CancellationToken cancellationToken)
    {
        var command = new ResetPasswordCommand(email);

        return await mediator.Send(command, cancellationToken);
    }

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> Account(CancellationToken cancellationToken)
    {
        var user = HttpContext.User.GetCurrentAuthenticatedUserData();
        var request = new GetUserDetailsQuery(user);

        return Ok(await mediator.Send(request, cancellationToken));
    }
}