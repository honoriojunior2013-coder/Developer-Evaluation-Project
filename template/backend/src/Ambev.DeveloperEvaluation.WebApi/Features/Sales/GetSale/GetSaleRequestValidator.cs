using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebAPI.Features.Sales.GetSale;

public class GetSaleRequestValidator : AbstractValidator<GetSaleRequest>
{
    public GetSaleRequestValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Sale ID is required");
    }
}