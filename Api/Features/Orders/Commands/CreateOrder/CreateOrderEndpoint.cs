namespace Api.Features.Orders.Commands.CreateOrder
{
   
    using Api.Domain.Models;
    using Ardalis.ApiEndpoints;
    using MediatR;
    using Microsoft.AspNetCore.Mvc;
    using Swashbuckle.AspNetCore.Annotations;

    public class CreateOrderEndpoint : EndpointBaseAsync
        .WithRequest<CreateOrderRequest>
        .WithActionResult<CreateOrderResult>
    {
        private readonly IMediator _mediator;

        public CreateOrderEndpoint(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("api/orders")]
        [SwaggerOperation(
            Summary = "Creates a new Order",
            Description = "Creates a new Order",
            OperationId = "Orders_Create",
            Tags = new[] { "Orders" })
        ]
        public override async Task<ActionResult<CreateOrderResult>> HandleAsync(CreateOrderRequest request, CancellationToken cancellationToken = default)
        {
            return await _mediator.Send(new CreateOrderCommand { OrderRequest = request });

        }
    }
}
