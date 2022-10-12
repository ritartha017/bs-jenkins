using Endava.BookSharing.Application.Abstract;
using Endava.BookSharing.Application.AuthorManagement.CreateAuthor;
using Endava.BookSharing.Application.BookManagement.UpdateBook;
using Endava.BookSharing.Application.Exceptions;
using Endava.BookSharing.Application.FileManagement.CreateFile;
using Endava.BookSharing.Application.Models;
using Endava.BookSharing.Application.UnitTests.Shared;
using Endava.BookSharing.Domain.Abstractions;
using Endava.BookSharing.Domain.Entities;
using Endava.BookSharing.Domain.Enums;
using FluentAssertions;
using MediatR;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
namespace Endava.BookSharing.Application.UnitTests.BookManagement.UpdateBook;

public class UpdateBookCommandHandlerTests
{
    [Theory]
    [AutoMoqData]
    public async Task UpdateBook_Handle_ThrowsBookSharingAccessDeniedExceptionUserNotOwnerAndNotAdmin(
        Mock<IMediator> mediator,
        Mock<IBookRepository> bookRepository,
        Mock<IAuthorRepository> authorRepository,
        Mock<IGenreRepository> genreRepository,
        Mock<ILanguageRepository> languageRepository,
        Mock<IImageValidator> imageValidator)
    {
        var user = new AuthenticatedUser("1", new Roles[] { Roles.User });

        var command = new UpdateBookCommand("book", user);
        var handler = new UpdateBookCommandHandler(
            mediator.Object, bookRepository.Object, authorRepository.Object,
            genreRepository.Object, languageRepository.Object, imageValidator.Object);

        imageValidator.Setup(x => x.IsValidImage(It.IsAny<IFileData>()))
            .Returns(true);

        bookRepository.Setup(x => x.GetByIdAsync(It.IsAny<string>(), CancellationToken.None))
            .ReturnsAsync(new Book() { OwnerId = "another" });

        var exception = await Record.ExceptionAsync(() => handler.Handle(command, CancellationToken.None));

        Assert.IsType<BookSharingAccessDeniedException>(exception);
    }

    [Theory]
    [AutoMoqData]
    public async Task UpdateBook_Handle_ThrowsBookSharingNotFoundExceptionWhenBookNotFoundWithId(
        Mock<IMediator> mediator,
        Mock<IBookRepository> bookRepository,
        Mock<IAuthorRepository> authorRepository,
        Mock<IGenreRepository> genreRepository,
        Mock<ILanguageRepository> languageRepository,
        Mock<IImageValidator> imageValidator)
    {
        var user = new AuthenticatedUser("1", new Roles[] { Roles.User });

        var command = new UpdateBookCommand("book", user);
        var handler = new UpdateBookCommandHandler(
            mediator.Object, bookRepository.Object, authorRepository.Object,
            genreRepository.Object, languageRepository.Object, imageValidator.Object);

        imageValidator.Setup(x => x.IsValidImage(It.IsAny<IFileData>()))
            .Returns(true);

        bookRepository.Setup(x => x.GetByIdAsync(It.IsAny<string>(), CancellationToken.None))
            .ReturnsAsync((Book)null!);

        var exception = await Record.ExceptionAsync(() => handler.Handle(command, CancellationToken.None));

        Assert.IsType<BookSharingNotFoundException>(exception);
    }

    [Theory]
    [AutoMoqData]
    public async Task UpdateBook_Handle_ThrowsBookSharingGenericExceptionWhenFileInvalid(
        Mock<IMediator> mediator,
        Mock<IBookRepository> bookRepository,
        Mock<IAuthorRepository> authorRepository,
        Mock<IGenreRepository> genreRepository,
        Mock<ILanguageRepository> languageRepository,
        Mock<IImageValidator> imageValidator)
    {
        var user = new AuthenticatedUser("1", new Roles[] { Roles.User, Roles.Admin });

        var command = new UpdateBookCommand("book", user)
        {
            AuthorId = "asd",
            FileType = "",
            RawFile = new byte[1]
        };

        var handler = new UpdateBookCommandHandler(
            mediator.Object, bookRepository.Object, authorRepository.Object,
            genreRepository.Object, languageRepository.Object, imageValidator.Object);

        imageValidator.Setup(x => x.IsValidImage(It.IsAny<IFileData>()))
            .Returns(false);

        bookRepository.Setup(x => x.GetByIdAsync(It.IsAny<string>(), CancellationToken.None))
            .ReturnsAsync(new Book() { OwnerId = "another" });


        var exception = await Record.ExceptionAsync(() => handler.Handle(command, CancellationToken.None));

        Assert.IsType<BookSharingGenericException>(exception);
    }

    [Theory]
    [AutoMoqData]
    public async Task UpdateBook_Handle_ThrowsBookSharingNotFoundExceptionWhenAuthorNotFoundWithId(
        Mock<IMediator> mediator,
        Mock<IBookRepository> bookRepository,
        Mock<IAuthorRepository> authorRepository,
        Mock<IGenreRepository> genreRepository,
        Mock<ILanguageRepository> languageRepository,
        Mock<IImageValidator> imageValidator)
    {
        var user = new AuthenticatedUser("1", new Roles[] { Roles.User, Roles.Admin });

        var command = new UpdateBookCommand("book", user)
        {
            AuthorId = "asd"
        };

        var handler = new UpdateBookCommandHandler(
            mediator.Object, bookRepository.Object, authorRepository.Object,
            genreRepository.Object, languageRepository.Object, imageValidator.Object);

        imageValidator.Setup(x => x.IsValidImage(It.IsAny<IFileData>()))
            .Returns(true);

        bookRepository.Setup(x => x.GetByIdAsync(It.IsAny<string>(), CancellationToken.None))
            .ReturnsAsync(new Book() { OwnerId = "another" });

        authorRepository.Setup(x => x.GetByIdAsync(It.IsAny<string>(), CancellationToken.None))
            .ReturnsAsync((Author)null!);

        var exception = await Record.ExceptionAsync(() => handler.Handle(command, CancellationToken.None));

        Assert.IsType<BookSharingNotFoundException>(exception);
    }

    [Theory]
    [AutoMoqData]
    public async Task UpdateBook_Handle_ThrowsBookSharingGenericExceptionWhenFailedToCreateAuthor(
        Mock<IMediator> mediator,
        Mock<IBookRepository> bookRepository,
        Mock<IAuthorRepository> authorRepository,
        Mock<IGenreRepository> genreRepository,
        Mock<ILanguageRepository> languageRepository,
        Mock<IImageValidator> imageValidator)
    {
        var user = new AuthenticatedUser("1", new Roles[] { Roles.User, Roles.Admin });

        var command = new UpdateBookCommand("book", user)
        {
            AuthorName = "Test"
        };

        var handler = new UpdateBookCommandHandler(
            mediator.Object, bookRepository.Object, authorRepository.Object,
            genreRepository.Object, languageRepository.Object, imageValidator.Object);

        imageValidator.Setup(x => x.IsValidImage(It.IsAny<IFileData>()))
            .Returns(true);

        bookRepository.Setup(x => x.GetByIdAsync(It.IsAny<string>(), CancellationToken.None))
            .ReturnsAsync(new Book() { OwnerId = "another" });

        authorRepository.Setup(x => x.IsNameUniqueAsync(It.IsAny<string>(), CancellationToken.None))
            .ReturnsAsync(true);

        mediator.Setup(x => x.Send(It.IsAny<CreateAuthorCommand>(), CancellationToken.None))
            .ReturnsAsync((string)null!);

        var exception = await Record.ExceptionAsync(() => handler.Handle(command, CancellationToken.None));

        Assert.IsType<BookSharingGenericException>(exception);
    }

    [Theory]
    [AutoMoqData]
    public async Task UpdateBook_Handle_ThrowsBookSharingGenericExceptionWhenAuthorNameAndIdNotSpecified(
        Mock<IMediator> mediator,
        Mock<IBookRepository> bookRepository,
        Mock<IAuthorRepository> authorRepository,
        Mock<IGenreRepository> genreRepository,
        Mock<ILanguageRepository> languageRepository,
        Mock<IImageValidator> imageValidator)
    {
        var user = new AuthenticatedUser("1", new Roles[] { Roles.User, Roles.Admin });

        var command = new UpdateBookCommand("book", user);

        var handler = new UpdateBookCommandHandler(
            mediator.Object, bookRepository.Object, authorRepository.Object,
            genreRepository.Object, languageRepository.Object, imageValidator.Object);

        bookRepository.Setup(x => x.GetByIdAsync(It.IsAny<string>(), CancellationToken.None))
            .ReturnsAsync(new Book() { OwnerId = "another" });

        var exception = await Record.ExceptionAsync(() => handler.Handle(command, CancellationToken.None));

        Assert.IsType<BookSharingGenericException>(exception);
    }

    [Theory]
    [AutoMoqData]
    public async Task UpdateBook_Handle_ThrowsBookSharingGenericExceptionWhenFailedToParsePublicationDate(
        Mock<IMediator> mediator,
        Mock<IBookRepository> bookRepository,
        Mock<IAuthorRepository> authorRepository,
        Mock<IGenreRepository> genreRepository,
        Mock<ILanguageRepository> languageRepository,
        Mock<IImageValidator> imageValidator)
    {
        var user = new AuthenticatedUser("1", new Roles[] { Roles.User, Roles.Admin });

        var command = new UpdateBookCommand("book", user)
        {
            PublicationDate = "asd/asd/asd",
            AuthorName = "Test"
        };

        var handler = new UpdateBookCommandHandler(
            mediator.Object, bookRepository.Object, authorRepository.Object,
            genreRepository.Object, languageRepository.Object, imageValidator.Object);

        imageValidator.Setup(x => x.IsValidImage(It.IsAny<IFileData>()))
            .Returns(true);

        bookRepository.Setup(x => x.GetByIdAsync(It.IsAny<string>(), CancellationToken.None))
            .ReturnsAsync(new Book() { OwnerId = "another" });

        authorRepository.Setup(x => x.IsNameUniqueAsync(It.IsAny<string>(), CancellationToken.None))
            .ReturnsAsync(true);

        mediator.Setup(x => x.Send(It.IsAny<CreateAuthorCommand>(), CancellationToken.None))
            .ReturnsAsync(string.Empty);

        var exception = await Record.ExceptionAsync(() => handler.Handle(command, CancellationToken.None));

        Assert.IsType<BookSharingGenericException>(exception);
    }

    [Theory]
    [AutoMoqData]
    public async Task UpdateBook_Handle_ThrowsBookSharingNotFoundExceptionWhenLanguageNotFoundWithId(
        Mock<IMediator> mediator,
        Mock<IBookRepository> bookRepository,
        Mock<IAuthorRepository> authorRepository,
        Mock<IGenreRepository> genreRepository,
        Mock<ILanguageRepository> languageRepository,
        Mock<IImageValidator> imageValidator)
    {
        var user = new AuthenticatedUser("1", new Roles[] { Roles.User, Roles.Admin });

        var command = new UpdateBookCommand("book", user)
        {
            LanguageId = "language",
            PublicationDate = "11/01/2022",
            AuthorName = "Test"
        };

        var handler = new UpdateBookCommandHandler(
            mediator.Object, bookRepository.Object, authorRepository.Object,
            genreRepository.Object, languageRepository.Object, imageValidator.Object);

        imageValidator.Setup(x => x.IsValidImage(It.IsAny<IFileData>()))
            .Returns(true);

        bookRepository.Setup(x => x.GetByIdAsync(It.IsAny<string>(), CancellationToken.None))
            .ReturnsAsync(new Book() { OwnerId = "another" });

        authorRepository.Setup(x => x.IsNameUniqueAsync(It.IsAny<string>(), CancellationToken.None))
            .ReturnsAsync(true);

        mediator.Setup(x => x.Send(It.IsAny<CreateAuthorCommand>(), CancellationToken.None))
            .ReturnsAsync(string.Empty);

        languageRepository.Setup(x => x.GetByIdAsync(It.IsAny<string>(), CancellationToken.None))
            .ReturnsAsync((Language)null!);

        var exception = await Record.ExceptionAsync(() => handler.Handle(command, CancellationToken.None));

        Assert.IsType<BookSharingNotFoundException>(exception);
    }

    [Theory]
    [AutoMoqData]
    public async Task UpdateBook_Handle_ThrowsBookSharingNotFoundExceptionWhenGenresAreNull(
        Mock<IMediator> mediator,
        Mock<IBookRepository> bookRepository,
        Mock<IAuthorRepository> authorRepository,
        Mock<IGenreRepository> genreRepository,
        Mock<ILanguageRepository> languageRepository,
        Mock<IImageValidator> imageValidator)
    {
        var user = new AuthenticatedUser("1", new Roles[] { Roles.User, Roles.Admin });

        var command = new UpdateBookCommand("book", user)
        {
            LanguageId = "language",
            PublicationDate = "11/01/2022",
            AuthorName = "Test",
            GenreIds = new[] { "asd", "asd", "asd" }
        };

        var handler = new UpdateBookCommandHandler(
            mediator.Object, bookRepository.Object, authorRepository.Object,
            genreRepository.Object, languageRepository.Object, imageValidator.Object);

        imageValidator.Setup(x => x.IsValidImage(It.IsAny<IFileData>()))
            .Returns(true);

        bookRepository.Setup(x => x.GetByIdAsync(It.IsAny<string>(), CancellationToken.None))
            .ReturnsAsync(new Book() { OwnerId = "another" });

        authorRepository.Setup(x => x.IsNameUniqueAsync(It.IsAny<string>(), CancellationToken.None))
            .ReturnsAsync(true);

        mediator.Setup(x => x.Send(It.IsAny<CreateAuthorCommand>(), CancellationToken.None))
            .ReturnsAsync(string.Empty);

        languageRepository.Setup(x => x.GetByIdAsync(It.IsAny<string>(), CancellationToken.None))
            .ReturnsAsync(new Language());

        genreRepository.Setup(x => x.GetGenresByIdsAsync(It.IsAny<ICollection<string>>(), CancellationToken.None))
            .ReturnsAsync((ICollection<Genre>)null!);

        var exception = await Record.ExceptionAsync(() => handler.Handle(command, CancellationToken.None));

        Assert.IsType<BookSharingNotFoundException>(exception);
    }

    [Theory]
    [AutoMoqData]
    public async Task UpdateBook_Handle_ThrowsBookSharingNotFoundExceptionWhenNotFoundAllGenres(
        Mock<IMediator> mediator,
        Mock<IBookRepository> bookRepository,
        Mock<IAuthorRepository> authorRepository,
        Mock<IGenreRepository> genreRepository,
        Mock<ILanguageRepository> languageRepository,
        Mock<IImageValidator> imageValidator)
    {
        var user = new AuthenticatedUser("1", new Roles[] { Roles.User, Roles.Admin });

        var command = new UpdateBookCommand("book", user)
        {
            LanguageId = "language",
            PublicationDate = "11/01/2022",
            AuthorName = "Test",
            GenreIds = new[] { "asd", "asd", "asd" }
        };

        var handler = new UpdateBookCommandHandler(
            mediator.Object, bookRepository.Object, authorRepository.Object,
            genreRepository.Object, languageRepository.Object, imageValidator.Object);

        imageValidator.Setup(x => x.IsValidImage(It.IsAny<IFileData>()))
            .Returns(true);

        bookRepository.Setup(x => x.GetByIdAsync(It.IsAny<string>(), CancellationToken.None))
            .ReturnsAsync(new Book() { OwnerId = "another" });

        authorRepository.Setup(x => x.IsNameUniqueAsync(It.IsAny<string>(), CancellationToken.None))
            .ReturnsAsync(true);

        mediator.Setup(x => x.Send(It.IsAny<CreateAuthorCommand>(), CancellationToken.None))
            .ReturnsAsync(string.Empty);

        languageRepository.Setup(x => x.GetByIdAsync(It.IsAny<string>(), CancellationToken.None))
            .ReturnsAsync(new Language());

        genreRepository.Setup(x => x.GetGenresByIdsAsync(It.IsAny<ICollection<string>>(), CancellationToken.None))
            .ReturnsAsync(new List<Genre>() { new Genre() });

        var exception = await Record.ExceptionAsync(() => handler.Handle(command, CancellationToken.None));

        Assert.IsType<BookSharingNotFoundException>(exception);
    }

    [Theory]
    [AutoMoqData]
    public async Task UpdateBook_Handle_ThrowsBookSharingNotFoundExceptionWhenFailedToUpdateBook(
        Mock<IMediator> mediator,
        Mock<IBookRepository> bookRepository,
        Mock<IAuthorRepository> authorRepository,
        Mock<IGenreRepository> genreRepository,
        Mock<ILanguageRepository> languageRepository,
        Mock<IImageValidator> imageValidator)
    {
        var user = new AuthenticatedUser("1", new Roles[] { Roles.User, Roles.Admin });

        var command = new UpdateBookCommand("book", user)
        {
            LanguageId = "language",
            PublicationDate = "11/01/2022",
            AuthorName = "Test",
            GenreIds = new[] { "asd", "asd", "asd" }
        };

        var handler = new UpdateBookCommandHandler(
            mediator.Object, bookRepository.Object, authorRepository.Object,
            genreRepository.Object, languageRepository.Object, imageValidator.Object);

        imageValidator.Setup(x => x.IsValidImage(It.IsAny<IFileData>()))
            .Returns(true);

        bookRepository.Setup(x => x.GetByIdAsync(It.IsAny<string>(), CancellationToken.None))
            .ReturnsAsync(new Book() { OwnerId = "another" });

        authorRepository.Setup(x => x.IsNameUniqueAsync(It.IsAny<string>(), CancellationToken.None))
            .ReturnsAsync(true);

        mediator.Setup(x => x.Send(It.IsAny<CreateAuthorCommand>(), CancellationToken.None))
            .ReturnsAsync(string.Empty);

        languageRepository.Setup(x => x.GetByIdAsync(It.IsAny<string>(), CancellationToken.None))
            .ReturnsAsync(new Language());

        genreRepository.Setup(x => x.GetGenresByIdsAsync(It.IsAny<ICollection<string>>(), CancellationToken.None))
            .ReturnsAsync(new List<Genre>() { new Genre(), new Genre(), new Genre() });

        bookRepository.Setup(x => x.UpdateAsync(It.IsAny<Book>(), It.IsAny<bool>(), CancellationToken.None))
            .ReturnsAsync(false);

        var exception = await Record.ExceptionAsync(() => handler.Handle(command, CancellationToken.None));

        Assert.IsType<BookSharingGenericException>(exception);
    }

    [Theory]
    [AutoMoqData]
    public async Task UpdateBook_Handle_ReturnsUnit(
        Mock<IMediator> mediator,
        Mock<IBookRepository> bookRepository,
        Mock<IAuthorRepository> authorRepository,
        Mock<IGenreRepository> genreRepository,
        Mock<ILanguageRepository> languageRepository,
        Mock<IImageValidator> imageValidator)
    {
        var user = new AuthenticatedUser("1", new Roles[] { Roles.User, Roles.Admin });

        var command = new UpdateBookCommand("book", user)
        {
            LanguageId = "language",
            PublicationDate = "11/01/2022",
            AuthorName = "Test",
            GenreIds = new[] { "asd", "asd", "asd" }
        };

        var handler = new UpdateBookCommandHandler(
            mediator.Object, bookRepository.Object, authorRepository.Object,
            genreRepository.Object, languageRepository.Object, imageValidator.Object);

        imageValidator.Setup(x => x.IsValidImage(It.IsAny<IFileData>()))
            .Returns(true);

        bookRepository.Setup(x => x.GetByIdAsync(It.IsAny<string>(), CancellationToken.None))
            .ReturnsAsync(new Book() { OwnerId = "another" });

        authorRepository.Setup(x => x.IsNameUniqueAsync(It.IsAny<string>(), CancellationToken.None))
            .ReturnsAsync(true);

        mediator.Setup(x => x.Send(It.IsAny<CreateFileCommand>(), CancellationToken.None))
            .ReturnsAsync(string.Empty);

        mediator.Setup(x => x.Send(It.IsAny<CreateAuthorCommand>(), CancellationToken.None))
            .ReturnsAsync(string.Empty);

        languageRepository.Setup(x => x.GetByIdAsync(It.IsAny<string>(), CancellationToken.None))
            .ReturnsAsync(new Language());

        genreRepository.Setup(x => x.GetGenresByIdsAsync(It.IsAny<ICollection<string>>(), CancellationToken.None))
            .ReturnsAsync(new List<Genre>() { new Genre(), new Genre(), new Genre() });

        bookRepository.Setup(x => x.UpdateAsync(It.IsAny<Book>(), It.IsAny<bool>(), CancellationToken.None))
            .ReturnsAsync(true);

        Func<Task> act = async () =>
        {
            await handler.Handle(command, CancellationToken.None);
        };

        await act.Should().NotThrowAsync();
    }
}
