using FluentValidation;
using Hypesoft.Application.Commands.Products;

namespace Hypesoft.Application.Validators.Products;

public class UpdateProductStockValidator : AbstractValidator<UpdateProductStockCommand>
{
    public UpdateProductStockValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.StockQuantity).GreaterThanOrEqualTo(0);
    }
}