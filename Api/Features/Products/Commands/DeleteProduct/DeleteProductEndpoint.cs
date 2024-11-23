using Api.Domain.Models;
using Ardalis.ApiEndpoints;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Api.Features.Products.Commands.DeleteProduct
{
    public class DeleteProductEndpoint : EndpointBaseAsync
        .WithRequest<Guid>
        .WithActionResult
    {
        private readonly IMediator _mediator;

        public DeleteProductEndpoint(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpDelete("api/products/{id}")]
        [SwaggerOperation(
            Summary = "Deletes a Product",
            Description = "Deletes a Product by ID",
            OperationId = "Products_Delete",
            Tags = new[] { "Products" })
        ]
        public override async Task<ActionResult> HandleAsync([FromRoute] Guid id, CancellationToken cancellationToken = default)
        {
            return await _mediator.Send(new DeleteProductCommand { ProductId = id });
            
        }
    }

   
}
