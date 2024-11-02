
using Api.Features.Common.Services.Storage;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Features.Pictures.Commands.CreatePicture
{
    public class CreatePictureCommand : IRequest<ActionResult<Guid>>
    {
        public IFormFile Picture { get; set; }
    }

    public class CreatePictureCommandHandler : IRequestHandler<CreatePictureCommand, ActionResult<Guid>>
    {
        private readonly IBlobService _blobService;
   

        public CreatePictureCommandHandler(IBlobService blobService)
        {
            _blobService = blobService;
          
        }

        public async Task<ActionResult<Guid>> Handle(CreatePictureCommand request, CancellationToken cancellationToken)
        {
            var file = request.Picture;
            if (file == null || file.Length == 0)
            {
                return new BadRequestObjectResult("File is empty.");
            }

            using var stream = file.OpenReadStream();
            Guid fileId = await _blobService.UploadAsync(stream, file.ContentType);

          
            return new OkObjectResult(fileId);
        }
    }
}
