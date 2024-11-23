using Api.Features.Products.Queries.GetAllProducts;
using Api.Features.Products.Queries.GetProducts;
using Ardalis.ApiEndpoints;
using Azure.Core;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Api.Features.Products.Queries.GetAllProducts
{
    public class GetAllProductEndpoint : EndpointBaseAsync.WithoutRequest.WithResult<ActionResult<List<ProductResult>>>
    {
        private readonly IMediator _mediator;

        public GetAllProductEndpoint(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("api/products")]
        [SwaggerOperation(
            Summary = "Get All Products",
            Description = "Retrieve all products from the database",
            OperationId = "Products_GetAll",
            Tags = new[] { "Products" })
        ]
        public override async Task<ActionResult<List<ProductResult>>> HandleAsync(CancellationToken cancellationToken = default)
        {

            return  await _mediator.Send(new GetProductsQuery {  });
        }
    }
}
