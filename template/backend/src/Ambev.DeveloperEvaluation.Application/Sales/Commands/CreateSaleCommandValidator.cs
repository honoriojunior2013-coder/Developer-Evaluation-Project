using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Sales.Commands;

public class CreateSaleCommandValidator : AbstractValidator<CreateSaleCommand>
{
    public CreateSaleCommandValidator()
    {
        RuleFor(x => x.Customer).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Branch).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Items).NotEmpty().WithMessage("Sale must have at least one item.");
        RuleForEach(x => x.Items).SetValidator(new CreateSaleItemDtoValidator());
    }
}

public class CreateSaleItemDtoValidator : AbstractValidator<CreateSaleItemDto>
{
    public CreateSaleItemDtoValidator()
    {
        RuleFor(x => x.ProductName).NotEmpty();
        RuleFor(x => x.UnitPrice).GreaterThan(0);
        
        // Max 20 items per product
        RuleFor(x => x.Quantity)
            .InclusiveBetween(1, 20)
            .WithMessage("Quantity for each product must be between 1 and 20 items.");
    }
}