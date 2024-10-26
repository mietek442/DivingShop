using Api.Features.Common.Services.Storage;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading.Tasks;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FilesController : ControllerBase
    {
        private readonly IBlobService _blobService;
        private readonly ILogger<FilesController> _logger;

        public FilesController(IBlobService blobService, ILogger<FilesController> logger)
        {
            _blobService = blobService;
            _logger = logger;
        }

        /// <summary>
        /// Uploads a file to blob storage.
        /// </summary>
        /// <param name="file">The file to upload.</param>
        /// <returns>The unique file identifier.</returns>
        [HttpPost]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("File is empty.");
            }

            using var stream = file.OpenReadStream();
            Guid fileId = await _blobService.UploadAsync(stream, file.ContentType);

            _logger.LogInformation($"File uploaded with ID: {fileId}");
            return Ok(fileId);
        }

        /// <summary>
        /// Downloads a file from blob storage by its ID.
        /// </summary>
        /// <param name="fileId">The unique file identifier.</param>
        /// <returns>The file stream.</returns>
        [HttpGet("{fileId}")]
        public async Task<IActionResult> DownloadFile(Guid fileId)
        {
            var fileResponse = await _blobService.DowloadAsync(fileId);
            if (fileResponse == null)
            {
                return NotFound("File not found.");
            }

            _logger.LogInformation($"File downloaded with ID: {fileId}");
            return File(fileResponse.stream, fileResponse.ContentType);
        }

        /// <summary>
        /// Deletes a file from blob storage by its ID.
        /// </summary>
        /// <param name="fileId">The unique file identifier.</param>
        /// <returns>No content if successful.</returns>
        [HttpDelete("{fileId}")]
        public async Task<IActionResult> DeleteFile(Guid fileId)
        {
            await _blobService.DeleteAsync(fileId);

            _logger.LogInformation($"File deleted with ID: {fileId}");
            return NoContent();
        }
    }
}
