using Api.Features.Orders.Commands.ChangeStatusOrder;

using Api.Features.Orders.Queries.GetOrderById;
using Api.Features.Products.Commands.UpdateProduct;
using Ardalis.ApiEndpoints;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading;
using System.Threading.Tasks;

namespace Api.Features.Orders.Commands.ChangeStatusOrder
{
    public class ChangeStatusOrderEndpoint : EndpointBaseAsync
        .WithRequest<ChangeStatusOrderRequest>
        .WithActionResult<OrderByIdResult>
    {
        private readonly IMediator _mediator;

        public ChangeStatusOrderEndpoint(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPut("api/orders/{id}/status")]
        [SwaggerOperation(
            Summary = "Changes the status of an existing Order",
            Description = "Changes the status of an existing Order by ID",
            OperationId = "Orders_ChangeStatus",
            Tags = new[] { "Orders" })
        ]
        public override async Task<ActionResult<OrderByIdResult>> HandleAsync(
            ChangeStatusOrderRequest request,
            CancellationToken cancellationToken = default)
        {
            return await _mediator.Send(new UpdateOrderStatusCommand { OrderId = request.Id, NewStatus = request.OrderStatus });
        }
    }
}
