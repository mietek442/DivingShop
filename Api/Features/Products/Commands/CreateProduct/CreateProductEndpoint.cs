using Api.Domain.Models;
using Ardalis.ApiEndpoints;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Api.Features.Products.Commands.CreateProduct
{
    public class CreateProductEndpoint : EndpointBaseAsync
        .WithRequest<CreateProductRequest>
        .WithActionResult<Product>
    {
        private readonly IMediator _mediator;

        public CreateProductEndpoint(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("api/products")]
        [SwaggerOperation(
            Summary = "Creates a new Product",
            Description = "Creates a new Product",
            OperationId = "Products_Create",
            Tags = new[] { "Products" })
        ]
        public override async Task<ActionResult<Product>> HandleAsync(CreateProductRequest request, CancellationToken cancellationToken = default)
        {
            return await _mediator.Send(new CreateProductCommand { ProductRequest = request });
        }
    }
}
