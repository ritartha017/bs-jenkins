using Endava.BookSharing.Application.Models.Requests;
using Endava.BookSharing.Application.Models.Validators;
using Endava.BookSharing.Application.UnitTests.Shared.Extensions;
using FluentValidation.Results;
using System.Collections.Generic;
using Xunit;

namespace Endava.BookSharing.Application.UnitTests.Models.Validators;

public class CreateBookValidatorTests
{
    #region Validators
    private static ValidationResult? RunValidation(CreateBookRequest createBookRequest)
    {
        var validator = new CreateBookValidator();
        var result = validator.Validate(createBookRequest);
        return result;
    }
    #endregion

    #region BookPublicationDateTests
    [Theory]
    [InlineData("12/12/2012")]
    [InlineData("01/01/2001")]
    [InlineData("01/05/2022")]
    public void CreateBookValidator_WhenGivenAValidDate_Accepts(string date)
    {
        var result = RunValidation(new CreateBookRequest() { PublicationDate = date });
        result.HasNotErrorMessage("Invalid date, please try again with a valid date in the format of MM/DD/YYYY.");
    }

    [Theory]
    [InlineData("12.12.2012")]
    [InlineData("12-12-2012")]
    [InlineData("12^12^2012")]
    [InlineData("12|12|2012")]
    public void CreateBookValidator_WhenInvalidSeparators_Fails(string date)
    {
        var result = RunValidation(new CreateBookRequest() { PublicationDate = date });
        result.AssertFailedValidation("Invalid date, please try again with a valid date in the format of MM/DD/YYYY.");
    }

    [Theory]
    [InlineData("31.13.2013")]
    [InlineData("12.00.2013")]
    [InlineData("12.01.2050")]
#pragma warning disable S4144 // Methods should not have identical implementations
    public void CreateBookValidator_WhenInvalidDayOrMonthOrYearValues_Fails(string date)
#pragma warning restore S4144 // Methods should not have identical implementations
    {
        var result = RunValidation(new CreateBookRequest() { PublicationDate = date });
        result.AssertFailedValidation("Invalid date, please try again with a valid date in the format of MM/DD/YYYY.");
    }

    [Theory]
    [InlineData("abracadabra")]
    [InlineData("testingissoborring")]
    [InlineData("//@12.43/.2001")]
    [InlineData("12.12.201222222")]
    [InlineData("12330000")]
#pragma warning disable S4144 // Methods should not have identical implementations
    public void CreateBookValidator_WhenTotallyInvalidDataType_Fails(string date)
#pragma warning restore S4144 // Methods should not have identical implementations
    {
        var result = RunValidation(new CreateBookRequest() { PublicationDate = date });
        result.AssertFailedValidation("Invalid date, please try again with a valid date in the format of MM/DD/YYYY.");
    }
    #endregion

    #region BookGenreIdsTests
    [Theory]
    [InlineData(null)]
    public void CreateBookValidator_OnEmptyGenreIds_Fails(ICollection<string> genreIds)
    {
        var result = RunValidation(new CreateBookRequest() { GenreIds = genreIds });
        result.AssertFailedValidation("At least one genreID must be specifed.");
    }
    #endregion

    #region BookLanguageIdTests
    [Theory]
    [InlineData(null)]
    public void CreateBookValidator_NotGivenLanguageId_Fails(string languageId)
    {
        var result = RunValidation(new CreateBookRequest() { LanguageId = languageId });
        result.AssertFailedValidation("Language ID must be specifed.");
    }

    [Theory]
    [InlineData("a55791df-bee2-4266-9009-b6745be7ae81")]
    public void CreateBookValidator_GivenValidLanguageId_Accepts(string languageId)
    {
        var result = RunValidation(new CreateBookRequest() { LanguageId = languageId });
        result.HasNotErrorMessage("Language ID must be specifed.");
    }
    #endregion

    #region BookAuthorIdTests
    [Theory]
    [InlineData("123456789", "AuthorName")]
    [InlineData(null, "AuthorName")]
    [InlineData("123456789", null)]
    public void CreateBookValidator_WhenGivenAuthorIdAndAuthorName_Accepts(string authorId, string authorFullName)
    {
        var result = RunValidation(new CreateBookRequest() { AuthorId = authorId, AuthorFullName = authorFullName });
        result.HasNotErrorMessage("At least Author ID or Author FullName must be specified.");
    }

    [Theory]
    [InlineData(null, null)]
    public void CreateBookValidator_WhenNotGivenBothAuthorIdAndAuthorName_Fails(string authorId, string authorFullName)
    {
        var result = RunValidation(new CreateBookRequest() { AuthorId = authorId, AuthorFullName = authorFullName });
        result.AssertFailedValidation("At least Author ID or Author FullName must be specified.");
    }
    #endregion

    #region BookAuthorFullNameTests
    [Theory]
    [InlineData("Ted Kachinsky")]
    [InlineData("Ted Ted")]
    public void CreateBookValidator_WhenGivenValidLengthFullName_Accepts(string authorFullName)
    {
        var result = RunValidation(new CreateBookRequest() { AuthorFullName = authorFullName, AuthorId = null });
        result.HasNotErrorMessage("Invalid FullName.");
    }

    [Theory]
    [InlineData("123456ddddddddddddddddddddddfffffffffffffffffffddvvvvvvvddd")]
    public void CreateBookValidator_WhenGivenInvalidLengthFullName_Fails(string authorFullName)
    {
        var result = RunValidation(new CreateBookRequest() { AuthorFullName = authorFullName, AuthorId = null });
        result.AssertFailedValidation("Invalid FullName.");
    }
    #endregion
}
