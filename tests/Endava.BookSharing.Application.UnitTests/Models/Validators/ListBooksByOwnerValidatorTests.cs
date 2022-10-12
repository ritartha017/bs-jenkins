using Endava.BookSharing.Application.BookManagement.ListBooksByOwner;
using Endava.BookSharing.Application.Filters;
using Endava.BookSharing.Application.Models.Requests;
using Endava.BookSharing.Application.Models.Validators;
using Endava.BookSharing.Application.UnitTests.Shared.Extensions;
using FluentValidation.Results;
using Xunit;

namespace Endava.BookSharing.Application.UnitTests.Models.Validators;

public class ListBooksByOwnerValidatorTests
{
    private ValidationResult? RunValidation(ListBooksByOwnerQuery query)
    {
        var validator = new ListBooksByOwnerValidator();
        var result = validator.Validate(query);
        
        return result;
    }

    [Theory]
    [InlineData("12345678", -1)]
    [InlineData("12345678", -100)]
    [InlineData("12345678", 0)]
    [InlineData("12345678", -9999)]
    public void Validate_Page_WithNotValidPage(string ownerId, int page)
    {
        var filter = new PaginationFilter(page);
        var query = new ListBooksByOwnerQuery(ownerId, filter);
        
        var result = RunValidation(query);
        result.AssertFailedValidation("The page number cannot be less than 1");
    }
}