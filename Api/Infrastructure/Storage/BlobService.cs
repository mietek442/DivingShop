using Api.Features.Common.Services.Storage;
using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace Api.Infrastructure.Storage
{
    internal sealed class BlobService(BlobServiceClient blobServiceClient) : IBlobService
    {

        public async Task DeleteAsync(Guid fileId, CancellationToken cancellationToken = default)
        {

            var containerClient = blobServiceClient.GetBlobContainerClient("files");

            BlobClient blobClient = containerClient.GetBlobClient(fileId.ToString());

            await blobClient.DeleteIfExistsAsync(cancellationToken: cancellationToken);

        }

        public async Task<FileRespone> DowloadAsync(Guid fileId, CancellationToken cancellationToken = default)
        {
            var containerClient = blobServiceClient.GetBlobContainerClient("files");

            BlobClient blobClient = containerClient.GetBlobClient(fileId.ToString());

            Response<BlobDownloadResult> response = await blobClient.DownloadContentAsync(cancellationToken: cancellationToken);


            return new FileRespone(response.Value.Content.ToStream(), response.Value.Details.ContentType);
        }


        public async Task<Guid> UploadAsync(Stream stream, string contentType, CancellationToken cancellationToken = default)
        {
            var containerClient = blobServiceClient.GetBlobContainerClient("files");

            var fieldId = Guid.NewGuid();

            BlobClient blobClient = containerClient.GetBlobClient(fieldId.ToString());

            await blobClient.UploadAsync(stream, new BlobHttpHeaders { ContentType = contentType }, cancellationToken: cancellationToken);
            return fieldId;
        }
    }
}