
using Api.Features.Orders.Commands.GenerateOrderLabel;
using Api.Features.Orders.Queries.GetOrderById;
using Ardalis.ApiEndpoints;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Api.Features.Orders.Commands.GenereteOrderLabel
{
    public class GenereteOrderLabelEndpoint : EndpointBaseAsync
        .WithRequest<Guid>
        .WithResult<IResult>
    {
        private readonly IMediator _mediator;

        public GenereteOrderLabelEndpoint(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("api/orders/label/{id}")]
        [SwaggerOperation(
            Summary = "Generates label for an Order",
            Description = "Generates label for an Order by ID",
            OperationId = "Orders_GenerateLabel",
            Tags = new[] { "Orders" })
        ]
        public override async Task<IResult> HandleAsync([FromRoute(Name = "id")] Guid request, CancellationToken cancellationToken = default)
        {
            return await _mediator.Send(new GenerateOrderLabelCommand { OrderId = request }, cancellationToken);
        }
    }
}
