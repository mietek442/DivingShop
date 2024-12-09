using Api.Domain.Models;
using Ardalis.ApiEndpoints;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Api.Features.ProductParams.Commands.CreateProductParam
{
    public class CreateProductParamEndpoint : EndpointBaseAsync
        .WithRequest<CreateProductParamRequest>
        .WithActionResult<ProductParam>
    {
        private readonly IMediator _mediator;

        public CreateProductParamEndpoint(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("api/products/params/{id}")]
        [SwaggerOperation(
            Summary = "Creates a new Product Param",
            Description = "Creates a new Product Param",
            OperationId = "Products_Param_Create",
            Tags = new[] { "ProductsParam" })
        ]
        public override async Task<ActionResult<ProductParam>> HandleAsync(
            CreateProductParamRequest request,
            CancellationToken cancellationToken = default)
            
        {
            
            return await _mediator.Send(new CreateProductParamCommand
            {
                ProductId = request.Id,
                ProductParamRequest = request.CreateProductParamModelRequest
            });

           
        }
    }
}
