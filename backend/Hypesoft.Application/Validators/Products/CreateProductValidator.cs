using FluentValidation;
using Hypesoft.Application.Commands.Products;

namespace Hypesoft.Application.Validators.Products;

public class CreateProductValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(120);
        RuleFor(x => x.Description).MaximumLength(1000);
        RuleFor(x => x.Price).GreaterThanOrEqualTo(0);
        RuleFor(x => x.CategoryId).NotEmpty();
        RuleFor(x => x.StockQuantity).GreaterThanOrEqualTo(0);
    }
}