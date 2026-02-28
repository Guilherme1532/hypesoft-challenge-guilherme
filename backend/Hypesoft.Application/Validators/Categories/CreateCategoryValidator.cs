using FluentValidation;
using Hypesoft.Application.Commands;

namespace Hypesoft.Application.Validators;

public class CreateCategoryValidator : AbstractValidator<CreateCategoryCommand>
{
    public CreateCategoryValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(120);
        RuleFor(x => x.Description).MaximumLength(1000);
    }
}