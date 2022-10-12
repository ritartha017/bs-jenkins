using Endava.BookSharing.Application.WishlistManagement.AddBookToWishlist;
using Endava.BookSharing.Application.WishlistManagement.DeleteBookFromWishlist;
using Endava.BookSharing.Application.WishlistManagement.GetBooks;
using Endava.BookSharing.Presentation.Extensions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Endava.BookSharing.Presentation.Controllers;

[Route("[controller]")]
[ApiController]
public class WishlistController : ControllerBase
{
    private readonly IMediator mediator;

    public WishlistController(IMediator mediator)
    {
        this.mediator = mediator;
    }

    [Authorize]
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> AddBook([FromBody] string bookId, CancellationToken cancellationToken)
    {
        var currentUserId = HttpContext.User.GetUserId();
        await mediator.Send(new AddBookToWishlistCommand(bookId, currentUserId), cancellationToken);
        return Ok();
    }

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetBooks([FromQuery] int page, CancellationToken cancellationToken)
    {
        var query = new GetBooksFromWishlistQuery(page, HttpContext.User.GetUserId());
        var paginationList = await mediator.Send(query, cancellationToken);
        return Ok(paginationList);
    }

    [Authorize]
    [HttpDelete]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> DeleteBook([FromBody] string bookId, CancellationToken cancellationToken)
    {
        var currentUserId = HttpContext.User.GetUserId();
        await mediator.Send(new DeleteBookFromWishlistCommand(bookId, currentUserId), cancellationToken);
        return Ok();
    }
}