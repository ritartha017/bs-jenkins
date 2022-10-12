using Endava.BookSharing.Application.AuthorManagement.CreateAuthor;
using FluentValidation;

namespace Endava.BookSharing.Application.Models.Validators;

public class CreateAuthorValidator : AbstractValidator<CreateAuthorCommand>
{
    public CreateAuthorValidator()
    {
        RuleSet("Fullname rules", () =>
        {
            RuleFor(model => model.FullName)
                .NotEmpty()
                .WithMessage("Author name cannot be empty");
        });
        RuleSet("AddedById rules", () =>
        {
            RuleFor(model => model.AddedById)
                .NotEmpty()
                .WithMessage("AddedById property cannot be empty");
        });
    }
}