using Ambev.DeveloperEvaluation.Application.Sales.Commands;
using Ambev.DeveloperEvaluation.Application.Sales.Queries;
using Ambev.DeveloperEvaluation.WebAPI.Features.Sales.CancelSale;
using Ambev.DeveloperEvaluation.WebAPI.Features.Sales.CreateSale;
using Ambev.DeveloperEvaluation.WebAPI.Features.Sales.GetSale;
using Ambev.DeveloperEvaluation.WebAPI.Features.Sales.UpdateSale;
using CreateSaleApiResponse =   Ambev.DeveloperEvaluation.WebAPI.Features.Sales.CreateSale.CreateSaleResponse;



using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Ambev.DeveloperEvaluation.Application.Sales.DTOs;




namespace Ambev.DeveloperEvaluation.WebAPI.Features.Sales;

[ApiController]
[Route("api/[controller]")]
public class SalesController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public SalesController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    /// <summary>
    /// Creates a new sale
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(CreateSaleApiResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<CreateSaleApiResponse>> CreateSale(
        CreateSaleRequest request,
        CancellationToken cancellationToken)
    {
        var command = _mapper.Map<CreateSaleCommand>(request);
        var result = await _mediator.Send(command, cancellationToken);
        
        var response = _mapper.Map<CreateSaleApiResponse>(result);
        return CreatedAtAction(nameof(GetSale), new { id = response.Id }, response);
    }

    /// <summary>
    /// Gets sale by ID
    /// </summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(GetSaleResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<GetSaleResponse>> GetSale(
        Guid id,
        CancellationToken cancellationToken)
    {
        var query = new GetSaleByIdQuery(id);
        var result = await _mediator.Send(query, cancellationToken);

        if (result == null)
            return NotFound();

        var response = _mapper.Map<GetSaleResponse>(result);
        return Ok(response);
    }

    /// <summary>
    /// Updates an existing sale
    /// </summary>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(UpdateSaleResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<UpdateSaleResponse>> UpdateSale(
        Guid id,
        [FromBody] UpdateSaleRequest request,
        CancellationToken cancellationToken)
    {
        request.Id = id;
        var command = _mapper.Map<UpdateSaleCommand>(request);
        await _mediator.Send(command, cancellationToken);

        var query = new GetSaleByIdQuery(id);
        var result = await _mediator.Send(query, cancellationToken);
        
        var response = _mapper.Map<UpdateSaleResponse>(result);
        return Ok(response);
    }


    // I Forgot to create this
    /// <summary>
    /// Deletes a sale
    /// </summary>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteSale(
        Guid id,
        CancellationToken cancellationToken)
    {
        var command = new DeleteSaleCommand(id); 
        await _mediator.Send(command, cancellationToken);
        return NoContent();
    }

    /// <summary>
    /// Cancels a sale
    /// </summary>
    [HttpPost("{id:guid}/cancel")]
    [ProducesResponseType(typeof(CancelSaleResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CancelSaleResponse>> CancelSale(
        Guid id,
        CancellationToken cancellationToken)
    {
        var command = new CancelSaleCommand(id);
        await _mediator.Send(command, cancellationToken);

        var response = new CancelSaleResponse
        {
            Id = id,
            IsCancelled = true,
            CancelledAt = DateTime.UtcNow
        };

        return Ok(response);
    }

    /// <summary>
    /// Cancels a specific item in a sale
    /// </summary>
    [HttpPatch("{saleId:guid}/items/{itemId:guid}/cancel")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CancelSaleItem(
        Guid saleId,
        Guid itemId,
        CancellationToken cancellationToken)
    {
        var command = new CancelSaleItemCommand(saleId, itemId);
        await _mediator.Send(command, cancellationToken);
        return NoContent();
    }

    [HttpGet] // Endpoint: GET /api/Sales
    [ProducesResponseType(typeof(Ambev.DeveloperEvaluation.Application.Sales.Queries.PagedResult<SaleDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetSales([FromQuery] GetSalesQuery query, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(query, cancellationToken);
        
        return Ok(result);
    }
}