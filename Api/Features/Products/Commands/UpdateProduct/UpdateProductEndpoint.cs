using Api.Domain.Models;
using Api.Features.Products.Commands.Common.Models;
using Api.Features.Products.Commands.CreateProduct;
using Api.Features.Products.Queries.GetByIdProduct;
using Ardalis.ApiEndpoints;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Api.Features.Products.Commands.UpdateProduct
{
    public class UpdateProductEndpoint : EndpointBaseAsync
        .WithRequest<UpdateProductRequest>
        .WithActionResult<ProductByIdResult>
    {
        private readonly IMediator _mediator;
        public UpdateProductEndpoint(IMediator mediator)
        {
            _mediator = mediator;
    }
        [HttpPut("api/products/{id}")]
        [SwaggerOperation(
           Summary = "Updates an existing Product",
           Description = "Updates an existing Product by ID",
           OperationId = "Products_Update",
           Tags = new[] { "Products" })
       ]
        public async override Task<ActionResult<ProductByIdResult>> HandleAsync(UpdateProductRequest request, CancellationToken cancellationToken = default)
        {
            return await _mediator.Send(new UpdateProductCommand { Id = request.Id, UpdateProductModelRequest = request.UpdateProductModelRequest });

        }
    }
}
