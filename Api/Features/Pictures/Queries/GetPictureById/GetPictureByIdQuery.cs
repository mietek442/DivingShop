using Api.Features.Common.Services.Storage;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Api.Features.Pictures.Queries.GetPictureById
{
    public class GetPictureByIdQuery : IRequest<FileRespone>
    {
        public Guid Id { get; set; }
    }

    public class GetPictureByIdQueryHandler : IRequestHandler<GetPictureByIdQuery, FileRespone>
    {
        private readonly IBlobService _blobService;

        public GetPictureByIdQueryHandler(IBlobService blobService)
        {
            _blobService = blobService;
        }

        public async Task<FileRespone> Handle(GetPictureByIdQuery request, CancellationToken cancellationToken)
        {
            var fileResponse = await _blobService.DowloadAsync(request.Id);

            return fileResponse;
        }
    }
}
