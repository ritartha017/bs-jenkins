using Endava.BookSharing.Application.BookManagement.ListBooksByOwner;
using FluentValidation;

namespace Endava.BookSharing.Application.Models.Validators;

public class ListBooksByOwnerValidator : AbstractValidator<ListBooksByOwnerQuery>
{
    private readonly int MINIMAL_LENGHT = 1;
    public ListBooksByOwnerValidator()
    {
        RuleFor(model => model.Filter.PageNumber)
            .NotEmpty()
            .WithMessage("The page number cannot be empty");
        RuleFor(model => model.OwnerId)
            .NotEmpty()
            .WithMessage("The owner id cannot be empty");
        RuleFor(model => model.Filter.PageNumber)
            .LessThan(MINIMAL_LENGHT)
            .WithMessage("The page number cannot be less than 1");
    }
}