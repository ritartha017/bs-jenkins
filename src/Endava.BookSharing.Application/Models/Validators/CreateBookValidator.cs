using Endava.BookSharing.Application.Models.Requests;
using FluentValidation;
using System.Globalization;

namespace Endava.BookSharing.Application.Models.Validators;

public class CreateBookValidator : AbstractValidator<CreateBookRequest>
{
    public CreateBookValidator()
    {
        RuleFor(model => model.Title)
            .NotNull().NotEmpty()
            .Must(NotContainLeadingAndTrailingSpaces)
            .When(model => model.Title != null)
            .WithMessage("Only latin alphabet and special symbols are allowed.");
        RuleFor(model => model.Title)
            .Matches("^[A-Za-z0-9@#*&.?!,:;— \\-\\][({)}'\"\\.\\.\\.]*$")
            .WithMessage("Only latin alphabet and special symbols are allowed.");

        RuleFor(model => model.AuthorFullName)
            .Matches("(?=^.{7,61}$)^[a-zA-Z-]{3,30} [a-zA-Z-]{3,30}$")
            .WithMessage("Invalid FullName.");
        RuleFor(model => model.AuthorFullName)
            .Length(7, 61)
            .WithMessage("The FullName must have at least 7 characters and not more than 61.");
        RuleFor(model => model.AuthorFullName)
            .NotNull()
            .When(model => model.AuthorId == null)
            .WithMessage("At least Author ID or Author FullName must be specified.");

        RuleFor(model => model.AuthorId)
            .NotNull()
            .When(model => model.AuthorFullName == null)
            .WithMessage("At least Author ID or Author FullName must be specified.");

        RuleFor(model => model.PublicationDate)
            .Must(BeAValidDate)
            .WithMessage("Invalid date, please try again with a valid date in the format of MM/DD/YYYY.");

        RuleFor(model => model.GenreIds)
            .NotNull()
            .WithMessage("At least one genreID must be specifed.");

        RuleFor(model => model.LanguageId)
            .NotNull()
            .WithMessage("Language ID must be specifed.");

        RuleFor(model => model.File)
            .SetValidator(new FormFileValidator());
    }

    public static bool BeAValidDate(string date)
    {
        if (!DateTime.TryParseExact(date, "MM/dd/yyyy",
            CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime pDate))
        {
            return false;
        }
        return pDate <= DateTime.Today;
    }

    public static bool NotContainLeadingAndTrailingSpaces(string title)
        => title.Trim().Equals(title);
}
