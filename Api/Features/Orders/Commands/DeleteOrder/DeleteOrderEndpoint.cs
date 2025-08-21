using Ardalis.ApiEndpoints;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Api.Features.Orders.Commands.DeleteOrder
{
    public class DeleteOrderEndpoint : EndpointBaseAsync.WithRequest<Guid>.WithActionResult<Guid>
    {
        private readonly IMediator _mediator;
        public DeleteOrderEndpoint(IMediator mediator)
        {
            _mediator = mediator;   
        }
        [HttpDelete("{id}/api/orders/")]
        [SwaggerOperation(
            Summary = "Delete order",
            Description = "Delete Order by ID",
            OperationId = "Orders_Delete",
            Tags = new[] { "Orders" })
        ]
        public override async Task<ActionResult<Guid>> HandleAsync([FromRoute(Name = "id")]  Guid request, CancellationToken cancellationToken = default)
        {
            return await _mediator.Send(new DeleteOrderCommand { OrderId = request }, cancellationToken);
        }
    }
}
