using FluentValidation;
using Hypesoft.Application.Commands.Products;

namespace Hypesoft.Application.Validators.Products;

public class DeleteProductValidator : AbstractValidator<DeleteProductCommand>
{
    public DeleteProductValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Product id is required.");
    }
}