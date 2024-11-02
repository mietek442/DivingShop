using Ardalis.ApiEndpoints;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Api.Features.Pictures.Commands.DeletePicture
{
    public class DeletePictureEndpoint : EndpointBaseAsync
        .WithRequest<Guid>
        .WithActionResult<Guid>
    {
        private readonly IMediator _mediator;

        public DeletePictureEndpoint(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpDelete("api/files/pictures/{id}")]
        [SwaggerOperation(
            Summary = "Deletes a Picture",
            Description = "Deletes a Picture by ID",
            OperationId = "Files_DeletePicture",
            Tags = new[] { "Files" })
        ]
        public override async Task<ActionResult<Guid>> HandleAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _mediator.Send(new DeletePictureCommand { Id = id });
            
        
        }
    }
}
