using FluentValidation;
using Hypesoft.Application.Commands.Categories;

namespace Hypesoft.Application.Validators.Categories;

public class DeleteCategoryValidator : AbstractValidator<DeleteCategoryCommand>
{
    public DeleteCategoryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Category id is required.");
    }
}