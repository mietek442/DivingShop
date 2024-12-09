using Api.Domain.Models;
using Ardalis.ApiEndpoints;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Api.Features.ProductParams.Commands.DeleteProductParam
{
    public class DeleteProductParamEndpoint : EndpointBaseAsync
        .WithRequest<Guid>
        .WithActionResult
    {
        private readonly IMediator _mediator;

        public DeleteProductParamEndpoint(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpDelete("api/productparams/{id}")]
        [SwaggerOperation(
            Summary = "Deletes a Product Param",
            Description = "Deletes a Product Param by ID",
            OperationId = "ProductParams_Delete",
            Tags = new[] { "ProductParams" })
        ]
        public override async Task<ActionResult> HandleAsync([FromRoute] Guid id, CancellationToken cancellationToken = default)
        {
            return await _mediator.Send(new DeleteProductParamCommand { ProductParamId = id });
        }
    }
}
