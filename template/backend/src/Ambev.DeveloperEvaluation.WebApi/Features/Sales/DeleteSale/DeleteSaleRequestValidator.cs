using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebAPI.Features.Sales.DeleteSale;

public class DeleteSaleRequestValidator : AbstractValidator<DeleteSaleRequest>
{
    public DeleteSaleRequestValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Sale ID is required");
    }
}