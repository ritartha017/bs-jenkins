using Endava.BookSharing.Presentation.Models.Requests;
using Endava.BookSharing.Presentation.Models.Validators;
using Endava.BookSharing.Presentation.UnitTests.Shared.Models;
using FluentValidation.Results;
using System.Collections.Generic;
using Xunit;

namespace Endava.BookSharing.Presentation.UnitTests.Models.Validators;

public class BookUpdateValidatorTests
{
    private readonly BookUpdateRequest _bookUpdateRequest = new BookUpdateRequest()
    {
        Title = "pass",
        PublicationDate = "01/01/2022",
        Genres = new List<string>() { "adbc", "asdb" },
        AuthorName = "pass pass",
        LanguageId = "pass",
        Cover = new FormData()
    };

    #region Validators
    private static ValidationResult? RunValidation(BookUpdateRequest createBookRequest)
    {
        var validator = new BookUpdateValidator();
        var result = validator.Validate(createBookRequest);
        return result;
    }
    #endregion

    #region TitleValidation
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("1#213*)#!@#$%^&*()")]
    [InlineData("ăîasdâîăâăâîăsdfadf439483")]
    [InlineData("title    ")]
    public void UpdateBookValidator_WhenTitleNotValid_Rejects(string title)
    {
        _bookUpdateRequest.Title = title;
        var result = RunValidation(_bookUpdateRequest);
        Assert.False(result!.IsValid);
    }

    [Theory]
    [InlineData("asdbasdasd")]
    public void UpdateBookValidator_WhenTitleValid_Accepts(string title)
    {
        _bookUpdateRequest.Title = title;
        var result = RunValidation(_bookUpdateRequest);
        Assert.True(result!.IsValid);
    }
    #endregion

    #region LanguageIdValidation
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void UpdateBookValidator_WhenLanguageIdNotValid_Rejects(string languageId)
    {
        _bookUpdateRequest.LanguageId = languageId;
        var result = RunValidation(_bookUpdateRequest);
        Assert.False(result!.IsValid);
    }

    [Theory]
    [InlineData("asdbasdasd")]
    public void UpdateBookValidator_WhenLanguageIdValid_Accepts(string languageId)
    {
        _bookUpdateRequest.LanguageId = languageId;
        var result = RunValidation(_bookUpdateRequest);
        Assert.True(result!.IsValid);
    }
    #endregion

    #region AuthorNameValidation
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("pass")]
    [InlineData("1234567890")]
    [InlineData("passpasasas")]
    public void UpdateBookValidator_WhenAuthorNameNotValid_Rejects(string AuthorName)
    {
        _bookUpdateRequest.AuthorId = null!;
        _bookUpdateRequest.AuthorName = AuthorName;
        var result = RunValidation(_bookUpdateRequest);
        Assert.False(result!.IsValid);
    }

    [Theory]
    [InlineData("Marcu Lilian")]
    [InlineData("Hello Hellaa")]
    public void UpdateBookValidator_WhenAuthorNameValid_Accepts(string AuthorName)
    {
        _bookUpdateRequest.AuthorId = null!;
        _bookUpdateRequest.AuthorName = AuthorName;
        var result = RunValidation(_bookUpdateRequest);
        Assert.True(result!.IsValid);
    }
    #endregion

    #region AuthorIdValidation
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void UpdateBookValidator_WhenAuthorIdNotValid_Rejects(string AuthorId)
    {
        _bookUpdateRequest.AuthorId = AuthorId;
        _bookUpdateRequest.AuthorName = null!;
        var result = RunValidation(_bookUpdateRequest);
        Assert.False(result!.IsValid);
    }

    [Theory]
    [InlineData("pasasas")]
    public void UpdateBookValidator_WhenAuthorIdValid_Accepts(string AuthorId)
    {
        _bookUpdateRequest.AuthorId = AuthorId;
        _bookUpdateRequest.AuthorName = null!;
        var result = RunValidation(_bookUpdateRequest);
        Assert.True(result!.IsValid);
    }
    #endregion

    #region GenresValidation
    [Fact]
    public void UpdateBookValidator_WhenGenresNotValid_Rejects()
    {
        _bookUpdateRequest.Genres = new List<string>();
        var result = RunValidation(_bookUpdateRequest);
        Assert.False(result!.IsValid);
    }

    [Fact]
    public void UpdateBookValidator_WhenGenresValid_Accepts()
    {
        _bookUpdateRequest.Genres = new List<string>() { "passss" };
        var result = RunValidation(_bookUpdateRequest);
        Assert.True(result!.IsValid);
    }
    #endregion
}
