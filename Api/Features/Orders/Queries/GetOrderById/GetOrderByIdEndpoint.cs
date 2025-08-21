using Api.Features.Orders.Queries.GetAllOrders;
using Api.Features.Products.Queries.GetByIdProduct;
using Ardalis.ApiEndpoints;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using static Api.Features.Orders.Queries.GetOrderById.GetOrderByIdQueryHandler;

namespace Api.Features.Orders.Queries.GetOrderById
{
    public class GetOrderByIdEndpoint : EndpointBaseAsync
        .WithRequest<Guid>
        .WithResult<ActionResult<OrderByIDto>>
    {
        private readonly IMediator _mediator;

        public GetOrderByIdEndpoint(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("api/orders/{id}")]
        [SwaggerOperation(
            Summary = "Get Order by ID",
            Description = "Retrieve a specific order with its order items by ID",
            OperationId = "Orders_GetById",
            Tags = new[] { "Orders" })
        ]
        public override async Task<ActionResult<OrderByIDto>> HandleAsync(
            [FromRoute] Guid id,
            CancellationToken cancellationToken = default)
        {

            return await _mediator.Send(new GetOrderByIdQuery { OrderId = id }, cancellationToken);
        }
    }
}
