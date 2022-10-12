using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace Endava.BookSharing.Application.Models.Validators;

public class FormFileValidator : AbstractValidator<IFormFile>
{
    public FormFileValidator()
    {
        RuleFor(model => model.Length)
            .NotNull()
            .LessThanOrEqualTo(200000)
            .WithMessage("File size is larger than allowed(200KB)");
    }
}
