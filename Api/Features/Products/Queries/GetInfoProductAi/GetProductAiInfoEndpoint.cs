using Api.Features.Products.Commands.Common.Models;
using Api.Features.Products.Commands.UpdateProduct;
using Api.Features.Products.Queries.GetByIdProduct;
using Ardalis.ApiEndpoints;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Api.Features.Products.Queries.GetInfoProductAi
{
   
    public class GetProductAiInfoEndpoint : EndpointBaseAsync
        .WithRequest<ProductInfoRequest>
        .WithActionResult<ProductInfoAiResult>
    {
        private readonly IMediator _mediator;
        public GetProductAiInfoEndpoint(IMediator mediator)
        {
            _mediator = mediator;   
        }
        [HttpPut("api/products/info/{id}")]
        [SwaggerOperation(
            Summary = "Updates an existing Product",
            Description = "Updates an existing Product by ID",
            OperationId = "Products_Update",
            Tags = new[] { "Products" })
        ]
        public override async Task<ActionResult<ProductInfoAiResult>> HandleAsync(
            ProductInfoRequest request,
            CancellationToken cancellationToken = default)
        {
            return await _mediator.Send(new GetProductInfoAiQuery { Id = request.Id, Question = request.Question }, cancellationToken);
        }
    }
}
