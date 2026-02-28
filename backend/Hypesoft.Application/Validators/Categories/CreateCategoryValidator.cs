using FluentValidation;
using Hypesoft.Application.Commands.Categories;

namespace Hypesoft.Application.Validators.Categories;

public class CreateCategoryValidator : AbstractValidator<CreateCategoryCommand>
{
    public CreateCategoryValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(120);
        RuleFor(x => x.Description).MaximumLength(1000);
    }
}