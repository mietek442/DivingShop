using Api.Features.Products.Queries.GetAllProducts;
using Ardalis.ApiEndpoints;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Api.Features.Products.Queries.GetByIdProduct
{
    public class GetByIdProductEndpoint : EndpointBaseAsync.WithRequest<Guid>.WithResult<ActionResult<ProductResult>>
    {
        private readonly IMediator _mediator;

        public GetByIdProductEndpoint(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("api/products/{id}")]
        [SwaggerOperation(
            Summary = "Get Product by ID",
            Description = "Retrieve a product by its ID from the database",
            OperationId = "Products_GetById",
            Tags = new[] { "Products" })
        ]
        public override async Task<ActionResult<ProductResult>> HandleAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return  await _mediator.Send(new GetProductByIdQuery { Id = id });
            
        }
    }
}
