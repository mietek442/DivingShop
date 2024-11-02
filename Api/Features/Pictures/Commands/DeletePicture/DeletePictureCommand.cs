using Api.Features.Common.Services.Storage;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Features.Pictures.Commands.DeletePicture
{
    public class DeletePictureCommand : IRequest<ActionResult<Guid>>
    {
        public Guid Id { get; set; }
    }

    public class DeletePictureCommandHandler : IRequestHandler<DeletePictureCommand, ActionResult<Guid>>
    {
        private readonly IBlobService _blobService;

        public DeletePictureCommandHandler(IBlobService blobService)
        {
            _blobService = blobService;
        }

        public async Task<ActionResult<Guid>> Handle(DeletePictureCommand request, CancellationToken cancellationToken)
        {
            var fileResponse = await _blobService.DowloadAsync(request.Id);
            if (fileResponse == null)
            {
                return new NotFoundObjectResult("File not found.");
            }

            await _blobService.DeleteAsync(request.Id);
            return new NoContentResult(); 
        }
    }
}
