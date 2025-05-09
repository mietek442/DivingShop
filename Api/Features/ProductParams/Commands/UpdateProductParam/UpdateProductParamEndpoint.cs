using Api.Domain.Models;
using Api.Features.ProductParams.Commands.CreateProductParam;
using Ardalis.ApiEndpoints;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Api.Features.ProductParams.Commands.UpdateProductParam
{
    public class UpdateProductParamEndpoint : EndpointBaseAsync.WithRequest<UpdateProductParamRequest>.WithActionResult<ProductParam>
    {
        private readonly IMediator _mediator;

        public UpdateProductParamEndpoint(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPut("api/products/params/{id}")]
        [SwaggerOperation(
            Summary = "Updates an existing Product Param",
            Description = "Updates an existing Product Param",
            OperationId = "Products_Param_Update",
            Tags = new[] { "ProductsParam" })
        ]
        public override async Task<ActionResult<ProductParam>> HandleAsync(UpdateProductParamRequest request, CancellationToken cancellationToken = default)
        {
            return await _mediator.Send(new UpdateProductParamCommand
            {
                ProductParamId = request.Id,
                ProductParamRequest = request.UpdateProductParamModelRequest
            }, cancellationToken);

       
        }
    }
}
