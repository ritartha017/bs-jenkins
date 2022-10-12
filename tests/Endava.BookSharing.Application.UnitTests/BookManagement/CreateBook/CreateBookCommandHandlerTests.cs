using Endava.BookSharing.Application.Abstract;
using Endava.BookSharing.Application.BookManagement.CreateBook;
using Endava.BookSharing.Application.Exceptions;
using Endava.BookSharing.Application.Models.Requests;
using Endava.BookSharing.Application.UnitTests.Shared;
using Endava.BookSharing.Domain.Abstractions;
using Endava.BookSharing.Domain.Entities;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Moq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Endava.BookSharing.Application.UnitTests.BookManagement.CreateBook;

public class CreateBookCommandHandlerTests
{
    [Theory]
    [AutoMoqData]
    public async Task Handle_WhenGivenInvalidAuthorId_ThrowsBookSharingNotFoundException(
        CreateBookRequest request,
        IFileData image,
        Mock<IMediator> mediator,
        Mock<IBookRepository> bookRepository,
        Mock<IAuthorRepository> authorRepository,
        Mock<IGenreRepository> genreRepository,
        Mock<ILanguageRepository> languageRepository,
        Mock<IFileRepository> fileRepository)
    {
        request.AuthorId = "One";
        var command = new CreateBookCommand(request, "userId", image);
        var handler = new CreateBookCommandHandler(bookRepository.Object, authorRepository.Object, genreRepository.Object,
                                                   languageRepository.Object, fileRepository.Object, mediator.Object);

        authorRepository.Setup(x => x.GetByIdAsync(It.IsAny<string>(), CancellationToken.None))
                                     .ReturnsAsync(new Author { Id = "Two" } );

        await Assert.ThrowsAsync<BookSharingNotFoundException>(() => handler.Handle(command, CancellationToken.None));
    }

    [Theory]
    [AutoMoqData]
    public async Task Handle_WhenGivenInvalidGenreIds_ThrowsBookSharingNotFoundException(
        CreateBookRequest request,
        IFileData image,
        Mock<IMediator> mediator,
        Mock<IBookRepository> bookRepository,
        Mock<IAuthorRepository> authorRepository,
        Mock<IGenreRepository> genreRepository,
        Mock<ILanguageRepository> languageRepository,
        Mock<IFileRepository> fileRepository)
    {
        request.GenreIds = new Collection<string> { "one", "two" };
        var command = new CreateBookCommand(request, "userId", image);
        var handler = new CreateBookCommandHandler(bookRepository.Object, authorRepository.Object, genreRepository.Object,
                                                   languageRepository.Object, fileRepository.Object, mediator.Object);

        genreRepository.Setup(x => x.GetGenresByIdsAsync(It.IsAny<Collection<string>>(), CancellationToken.None))
                                    .ReturnsAsync((Collection<Genre>)null!);

        await Assert.ThrowsAsync<BookSharingNotFoundException>(() => handler.Handle(command, CancellationToken.None));
    }

    [Theory]
    [AutoMoqData]
    public async Task Handle_WhenGivenInvalidLanguageId_ThrowsBookSharingNotFoundException(
    CreateBookRequest request,
    IFileData image,
    Mock<IMediator> mediator,
    Mock<IBookRepository> bookRepository,
    Mock<IAuthorRepository> authorRepository,
    Mock<IGenreRepository> genreRepository,
    Mock<ILanguageRepository> languageRepository,
    Mock<IFileRepository> fileRepository)
    {
        request.LanguageId = "One";
        var command = new CreateBookCommand(request, "userId", image);
        var handler = new CreateBookCommandHandler(bookRepository.Object, authorRepository.Object, genreRepository.Object,
                                                   languageRepository.Object, fileRepository.Object, mediator.Object);

        languageRepository.Setup(x => x.GetByIdAsync(It.IsAny<string>(), CancellationToken.None))
                                       .ReturnsAsync((Language)null!);

        await Assert.ThrowsAsync<BookSharingNotFoundException>(() => handler.Handle(command, CancellationToken.None));
    }

    [Theory]
    [AutoMoqData]
    public async Task Handle_WhenGivenAllValidData_NotThrowAsync(
        Mock<IMediator> mediator,
        Mock<IBookRepository> bookRepository,
        Mock<IAuthorRepository> authorRepository,
        Mock<IGenreRepository> genreRepository,
        Mock<ILanguageRepository> languageRepository,
        Mock<IFileRepository> fileRepository,
        IFileData image,
        IFormFile imageFromReq)
    {
        var request = new CreateBookRequest()
        {
            Title = "T1",
            AuthorId = "A1",
            AuthorFullName = null!,
            PublicationDate = "12/12/2012",
            GenreIds = new[] { "G1", "G2" },
            File = imageFromReq,
            LanguageId = "L1",
        };
        var command = new CreateBookCommand(request, "userId", image);
        var handler = new CreateBookCommandHandler(bookRepository.Object, authorRepository.Object, genreRepository.Object,
                                                   languageRepository.Object, fileRepository.Object, mediator.Object);
        authorRepository.Setup(x => x.GetByIdAsync(It.IsAny<string>(), CancellationToken.None)).ReturnsAsync(new Author { Id = "A1" });
        genreRepository.Setup(x => x.GetGenresByIdsAsync(It.IsAny<ICollection<string>>(), CancellationToken.None)).ReturnsAsync(new Collection<Genre> { new Genre(), new Genre() });
        languageRepository.Setup(x => x.GetByIdAsync(It.IsAny<string>(), CancellationToken.None)).ReturnsAsync(new Language { Id = "L1" });
        fileRepository.Setup(x => x.CreateAsync(It.IsAny<Domain.Entities.File>(), CancellationToken.None)).ReturnsAsync("SomeString");
        bookRepository.Setup(x => x.CreateAsync(It.IsAny<Book>(), CancellationToken.None)).ReturnsAsync(true);

        Func<Task> result = async () => await handler.Handle(command, CancellationToken.None);

        await result.Should().NotThrowAsync();
    }
}
