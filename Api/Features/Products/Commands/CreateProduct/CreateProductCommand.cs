using Api.Domain.Models;
using Api.Features.Common.Services.Storage;
using Api.Features.Common.Services.UrlHelper;
using Api.Infrastructure.DbContext;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;


namespace Api.Features.Products.Commands.CreateProduct
{
    public class CreateProductCommand : IRequest<ActionResult<Product>>
    {
        public CreateProductRequest ProductRequest { get; set; }
    }

    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, ActionResult<Product>>
    {
        private readonly IBlobService _blobService;
        private readonly IApplicationContext _context;
        private readonly IUrlHelpers _urlHelpers;
        public CreateProductCommandHandler(IBlobService blobService, IApplicationContext context, IUrlHelpers urlHelpers)
        {
            _blobService = blobService;
            _urlHelpers = urlHelpers;
            _context = context;
        }

        public async Task<ActionResult<Product>> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {


            var product = new Product
            {
                Title = request.ProductRequest.Title,
                ShortDesc = request.ProductRequest.ShortDesc,
                Description = request.ProductRequest.Description,
                Manufacture = request.ProductRequest.Manufacture,
                Available =request.ProductRequest.Available,
                BasePrice = request.ProductRequest.BasePrice,
                Discount = request.ProductRequest.Discount,
               
                Size = request.ProductRequest.Size,
            };
            foreach (var image in request.ProductRequest.Images)
            {
                using var stream = image.OpenReadStream();

                var imageId = await _blobService.UploadAsync(stream, image.ContentType);
                var imageUrlApp = CreatePictureUrl(imageId, _urlHelpers);
                product.ImageUrls.Add(imageUrlApp.ToString());
            }
           
            _context.Products.Add(product);
            var res = await _context.SaveChangesAsync(cancellationToken);
            return new OkObjectResult(product);
        }
        private static string CreatePictureUrl(Guid? imgId, IUrlHelpers urlHelpers)
        {

            if (urlHelpers == null)
                throw new InvalidOperationException("UrlHelper is not set.");



            var url = urlHelpers.CreatePictureUrl(imgId);
            if (url == null)
            {
                return "Unable to generate the URL.";
            }


            return string.IsNullOrEmpty(url) ? "Unable to generate the URL." : url;
        }
    }

}
