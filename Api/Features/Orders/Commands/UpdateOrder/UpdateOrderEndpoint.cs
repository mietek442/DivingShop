using Api.Features.Orders.Commands.ChangeStatusOrder;
using Api.Features.Orders.Commands.UpdateOrder;
using Api.Features.Orders.Queries.GetOrderById;
using Ardalis.ApiEndpoints;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading;
using System.Threading.Tasks;

namespace Api.Features.Orders.Commands.UpdateOrder
{
    public class UpdateOrderEndpoint : EndpointBaseAsync
        .WithRequest<UpdateOrderRequest>
        .WithActionResult<UpdateOrderResult>
    {
        private readonly IMediator _mediator;

        public UpdateOrderEndpoint(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPut("api/orders/{id}")]
        [SwaggerOperation(
            Summary = "Updates an existing Order",
            Description = "Updates order details by ID",
            OperationId = "Orders_Update",
            Tags = new[] { "Orders" })
        ]
        public override  async Task<ActionResult<UpdateOrderResult>> HandleAsync(UpdateOrderRequest request, CancellationToken cancellationToken = default)
        {
            return await _mediator.Send(new UpdateOrderCommand { OrderId = request.Id, UpdateOrderRequestBody = request.UpdateOrderRequestBody });
        }
    }
}
