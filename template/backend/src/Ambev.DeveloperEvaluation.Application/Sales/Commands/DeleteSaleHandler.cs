using Ambev.DeveloperEvaluation.Domain.Repositories;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.Commands;

public class DeleteSaleHandler : IRequestHandler<DeleteSaleCommand, bool>
{
    private readonly ISaleRepository _repository;

    public DeleteSaleHandler(ISaleRepository repository)
    {
        _repository = repository;
    }

    public async Task<bool> Handle(DeleteSaleCommand request, CancellationToken cancellationToken)
    {
        var sale = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (sale == null) return false;

        _repository.Delete(sale);
        await _repository.SaveChangesAsync(cancellationToken);
        return true;
    }
}