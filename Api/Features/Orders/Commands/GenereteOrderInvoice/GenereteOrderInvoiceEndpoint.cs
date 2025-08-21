using Api.Features.Orders.Commands.GenerateOrderInvoice;
using Api.Features.Orders.Queries.GetOrderById;
using Ardalis.ApiEndpoints;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Api.Features.Orders.Commands.GenereteOrderInvoice
{
    public class GenereteOrderInvoiceEndpoint : EndpointBaseAsync
        .WithRequest<Guid>
            .WithResult<IResult>
    {
        private readonly IMediator _mediator;

        public GenereteOrderInvoiceEndpoint(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("api/orders/invoice/{id}")]
        [SwaggerOperation(
            Summary = "Generates invoice for an Order",
            Description = "Generates invoice for an Order by ID",
            OperationId = "Orders_GenerateInvoice",
            Tags = new[] { "Orders" })
        ]
        public override async Task<IResult> HandleAsync([FromRoute(Name = "id")] Guid request, CancellationToken cancellationToken = default)
        {
           return await _mediator.Send(new GenerateOrderInvoiceCommand { OrderId = request }, cancellationToken);
        }
    }
}
