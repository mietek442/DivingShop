using Api.Features.Common.Services.Storage;
using Api.Features.Pictures.Commands.CreatePicture;
using Ardalis.ApiEndpoints;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Api.Features.Pictures.Queries.GetPictureById
{
    public class GetPictureByIdEndpoint : EndpointBaseAsync.WithRequest<Guid>.WithResult<IActionResult>
    {
        private readonly IMediator _mediator;
        public GetPictureByIdEndpoint(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("api/files/{id}")]
        [SwaggerOperation(
            Summary = "Get Pictures By Id",
            Description = "Get Pictures By Id",
            OperationId = "Files_Picture",
            Tags = new[] { "Files" })
        ]
        public override async Task<IActionResult> HandleAsync([FromRoute] Guid id, CancellationToken cancellationToken = default)
        {
            var fileResult = await _mediator.Send(new GetPictureByIdQuery { Id = id });
            if (fileResult == null)
            {
                return NotFound("File not found.");
            }
            return File(fileResult.stream, fileResult.ContentType);
           
        }
    }
}
