using Ardalis.ApiEndpoints;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Api.Features.Pictures.Commands.CreatePicture
{
    public class CreatePicturesEndpoint : EndpointBaseAsync
        .WithRequest<IFormFile>
         .WithActionResult<Guid>
    {
        private readonly IMediator _mediator;

        public CreatePicturesEndpoint(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("api/files/pictures")]
        [SwaggerOperation(
            Summary = "Creates a new Pictures",
            Description = "Creates a new Pictures",
            OperationId = "Files_Picture",
            Tags = new[] { "Files" })
        ]
        public override async Task<ActionResult<Guid>> HandleAsync(IFormFile request, CancellationToken cancellationToken = default)
        {
            return await _mediator.Send(new CreatePictureCommand { Picture = request });
        }
    }
}
