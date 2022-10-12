using Endava.BookSharing.Application.Models.Validators;
using Endava.BookSharing.Presentation.Models.Requests;
using FluentValidation;
using System.Globalization;

namespace Endava.BookSharing.Presentation.Models.Validators;

public class BookUpdateValidator : AbstractValidator<BookUpdateRequest>
{
    public BookUpdateValidator()
    {
        RuleFor(model => model.Title)
            .NotNull()
            .NotEmpty()
            .WithMessage("Title cannot empty.");
        RuleFor(model => model.Title)
            .Must(NotContainLeadingAndTrailingSpaces)
            .When(x => x.Title is not null)
            .WithMessage("Only latin alphabet and special symbols are allowed.");
        RuleFor(model => model.Title)
            .Matches("^[A-Za-z0-9@#*&.?!,:;— \\-\\][({)}'\"\\.\\.\\.]*$")
            .WithMessage("Only latin alphabet and special symbols are allowed.");

        RuleFor(model => model.AuthorName)
            .Matches("(?=^.{7,61}$)^[a-zA-Z-]{3,30} [a-zA-Z-]{3,30}$")
            .WithMessage("Invalid FullName.");

        RuleFor(model => model.AuthorName)
            .Length(7, 61)
            .WithMessage("The FullName must have at least 7 characters and not more than 61.");

        RuleFor(model => model.AuthorName)
            .NotNull()
            .NotEmpty()
            .When(model => model.AuthorId == null)
            .WithMessage("At least Author ID or Author FullName must be specified.");

        RuleFor(model => model.AuthorId)
            .NotNull()
            .NotEmpty()
            .When(model => model.AuthorName == null)
            .WithMessage("At least Author ID or Author FullName must be specified.");

        RuleFor(model => model.LanguageId)
            .NotNull()
            .NotEmpty()
            .WithMessage("Language ID must be specified.");

        RuleFor(model => model.Genres)
            .NotNull()
            .NotEmpty()
            .WithMessage("At least one genreID must be specified.");

        RuleFor(model => model.PublicationDate)
            .NotNull()
            .Must(BeAValidDate)
            .WithMessage("Invalid date, please try again with a valid date in the format of DD/MM/YYYY.");

        RuleFor(model => model.Cover)
           .SetValidator(new FormFileValidator()!)
           .When(model => model.Cover != null);
    }
    public static bool BeAValidDate(string input)
    {
        if (!DateTime.TryParseExact(input, "dd/MM/yyyy",
            CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime pDate))
        {
            return false;
        }
        return pDate <= DateTime.Today;
    }
    public static bool NotContainLeadingAndTrailingSpaces(string title)
        => title.Trim().Equals(title);
}
