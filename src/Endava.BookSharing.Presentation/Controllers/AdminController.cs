using Endava.BookSharing.Application.Exceptions;
using Endava.BookSharing.Application.FeedbackManagement.RemoveFeedback;
using Endava.BookSharing.Presentation.Extensions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Endava.BookSharing.Presentation.Controllers;

[Route("[controller]")]
[ApiController]
public class AdminController : ControllerBase
{
    private readonly IMediator mediator;

    public AdminController(IMediator mediator)
    {
        this.mediator = mediator;
    }

    [Authorize]
    [HttpDelete("feedback")]
    public async Task<IActionResult> RemoveReview([FromQuery] string reviewId, CancellationToken cancellationToken)
    {
        if (!HttpContext.User.GetCurrentAuthenticatedUserData().IsAdminOrSuperAdmin)
        {
            throw new BookSharingAccessDeniedException("Request forbidden");
        }

        var query = new RemoveReviewCommand(reviewId);
        await mediator.Send(query, cancellationToken);

        return Ok();
    }
}
