using Api.Features.Products.Commands.Common.Models;
using Api.Features.Products.Queries.GetAllProducts;
using Ardalis.ApiEndpoints;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Api.Features.Products.Queries.GetByIdProduct
{
    public class GetByIdProductEndpoint : EndpointBaseAsync
        .WithRequest<Guid>
        .WithResult<ActionResult<ProductByIdResult>>
    {
        private readonly IMediator _mediator;

        public GetByIdProductEndpoint(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpGet("api/products/{id}")]

        [SwaggerOperation(
            Summary = "Get All Productss",
            Description = "Retrieve all products from the database",
            OperationId = "Products_GetAlls",
            Tags = new[] { "Products" })
        ]
        public override async Task<ActionResult<ProductByIdResult>> HandleAsync(Guid id, CancellationToken cancellationToken = default)
        {
            

           return await _mediator.Send(new GetProductByIdQuery { Id = id }, cancellationToken);

        }
    }
}
