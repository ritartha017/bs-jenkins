using Endava.BookSharing.Application.Abstract;
using Endava.BookSharing.Application.AuthorManagement.GetAuthorsList;
using Endava.BookSharing.Application.BookManagement.BookCover;
using Endava.BookSharing.Application.BookManagement.CreateBook;
using Endava.BookSharing.Application.BookManagement.DeleteBook;
using Endava.BookSharing.Application.BookManagement.GetBook;
using Endava.BookSharing.Application.BookManagement.ListBooks;
using Endava.BookSharing.Application.BookManagement.ListBooksByOwner;
using Endava.BookSharing.Application.BookManagement.UpdateBook;
using Endava.BookSharing.Application.Exceptions;
using Endava.BookSharing.Application.Extensions;
using Endava.BookSharing.Application.FileManagement.DeleteFile;
using Endava.BookSharing.Application.Filters;
using Endava.BookSharing.Application.GenresManagement.GetGenresList;
using Endava.BookSharing.Application.LanguageManagement.GetLanguagesList;
using Endava.BookSharing.Application.Models;
using Endava.BookSharing.Application.Models.Options;
using Endava.BookSharing.Application.Models.Requests;
using Endava.BookSharing.Application.ReviewManagement.GetReviews;
using Endava.BookSharing.Application.UserManagement.BookFeedback;
using Endava.BookSharing.Domain.Enums;
using Endava.BookSharing.Presentation.Extensions;
using Endava.BookSharing.Presentation.Models.Requests;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Endava.BookSharing.Presentation.Controllers;

[Route("[controller]")]
[ApiController]
public class BookController : ControllerBase
{
    private readonly IMediator mediator;
    private readonly IImageValidator imageValidator;
    private readonly BookCoverOptions bookCoverOptions;

    public BookController(IMediator mediator,
        IImageValidator imageValidator,
        IOptions<BookCoverOptions> bookCoverOptions)
    {
        this.mediator = mediator;
        this.imageValidator = imageValidator;
        this.bookCoverOptions = bookCoverOptions.Value;
    }

    [HttpGet("my")]
    [Authorize]
    public async Task<ActionResult> GetAllCourses([FromQuery] int page, CancellationToken cancellationToken)
    {
        var user = HttpContext.User.GetCurrentAuthenticatedUserData();
        var filter = new PaginationFilter(page);

        var query = new ListBooksByOwnerQuery(user.Id, filter);
        var response = await mediator.Send(query, cancellationToken);

        return Ok(response);
    }

    [Authorize]
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Create(
        [FromForm] CreateBookRequest request,
        CancellationToken cancellationToken)
    {
        var cover = new ImageData(request.File.ContentType, await request.File.GetBytes());
        var newCover = imageValidator.IsValidImage(cover)
            ? cover
            : throw new BookSharingGenericException("Invalid cover file format.");
        var ownerId = HttpContext.User.GetUserId();

        var command = new CreateBookCommand(request, ownerId, newCover);
        await mediator.Send(command, cancellationToken);

        return Ok();
    }

    [HttpGet("languages")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllLanguages(CancellationToken cancellationToken)
    {
        var query = new GetLanguagesListQuery();
        var response = await mediator.Send(query, cancellationToken);

        return Ok(response);
    }

    [HttpGet("authors")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllAuthors(CancellationToken cancellationToken)
    {
        var query = new GetAuthorsListQuery();
        var response = await mediator.Send(query, cancellationToken);

        return Ok(response);
    }

    [HttpGet("genres")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllGenres(CancellationToken cancellationToken)
    {
        var query = new GetGenresListQuery();
        var response = await mediator.Send(query, cancellationToken);

        return Ok(response);
    }

    [HttpGet("dropdowns")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllDropdowns(CancellationToken cancellationToken)
    {
        var authorsTask = mediator.Send(new GetAuthorsListQuery(), cancellationToken);
        var genresTask = mediator.Send(new GetGenresListQuery(), cancellationToken);
        var languagesTask = mediator.Send(new GetLanguagesListQuery(), cancellationToken);

        await Task.WhenAll(authorsTask, genresTask, languagesTask);

        var authors = await authorsTask;
        var genres = await genresTask;
        var languages = await languagesTask;

        var itemsList = new { authors, genres, languages };

        return Ok(itemsList);
    }

    [HttpDelete]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Delete([FromQuery] string id, [FromQuery] DeletionType type,
        CancellationToken cancellationToken)
    {
        var user = HttpContext.User.GetCurrentAuthenticatedUserData();
        switch (type)
        {
            case DeletionType.Cover:
                {
                    var command = new DeleteFileCommand(id, user.Id, user.Roles);
                    await mediator.Send(command, cancellationToken);
                    break;
                }
            case DeletionType.Full:
                {
                    var command = new DeleteBookCommand(id, user);
                    await mediator.Send(command, cancellationToken);
                    break;
                }
            default:
                return BadRequest("Invalid deletion type");
        }
        return Ok();
    }

    [HttpPut]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Update([FromQuery] string id, [FromForm] BookUpdateRequest newBookData,
        CancellationToken cancellationToken)
    {
        var user = HttpContext.User.GetCurrentAuthenticatedUserData();

        var command = new UpdateBookCommand(id, user)
        {
            Title = newBookData.Title,
            PublicationDate = newBookData.PublicationDate,
            GenreIds = newBookData.Genres,
            LanguageId = newBookData.LanguageId,
            AuthorId = newBookData.AuthorId,
            AuthorName = newBookData.AuthorName,
            FileType = newBookData.Cover?.ContentType,
            RawFile = newBookData.Cover?.OpenReadStream().ToByteArray()
        };

        await mediator.Send(command, cancellationToken);

        return Ok();
    }

    [HttpGet]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetBookDetails([FromQuery] string bookId, CancellationToken cancellationToken)
    {
        var user = HttpContext.User.GetCurrentAuthenticatedUserData();
        var command = new GetBookCommand(bookId, user);
        var result = await mediator.Send(command, cancellationToken);
        return Ok(result);
    }

    [HttpGet("feedback")]
    public async Task<IActionResult> GetReviews([FromQuery(Name = "book_id")] string bookId, [FromQuery] int page,
        CancellationToken cancellationToken)
    {
        var query = new GetReviewsQuery(bookId, page);
        var response = await mediator.Send(query, cancellationToken);

        return Ok(response);
    }

    [HttpGet]
    [Route("cover")]
    public async Task<IActionResult> GetBookCover([FromQuery(Name = "book_id")] string bookId,
        CancellationToken cancellationToken)
    {
        var command = new GetBookCoverQuery(bookId);
        var imageCover = await mediator.Send(command, cancellationToken);

        if (imageCover is null) return Redirect(bookCoverOptions.NoCoverUrl);

        return File(imageCover.Data.Raw, imageCover.Data.ContentType);
    }

    [HttpGet("available")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> AvailableBooks([FromQuery] FilterBooksRequest request, CancellationToken cancellationToken)
    {
        var availableBooks = new ListAvailableBooksQuery(request);
        var bookList = await mediator.Send(availableBooks, cancellationToken);
        return Ok(bookList);
    }
    
    [Authorize]
    [HttpPost("review")]
    public async Task<IActionResult> CreateReviewAsync([FromBody] CreateReviewRequest request, CancellationToken cancellationToken)
    {
        var currentUserId = HttpContext.User.GetUserId();
        var command = new CreateReviewCommand(request, currentUserId);

        await mediator.Send(command, cancellationToken);

        return Ok();
    }
}