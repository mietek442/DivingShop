using Api.Domain.Models;
using Api.Features.Common.Services.Storage;
using Api.Infrastructure.DbContext;
using MediatR;
using Microsoft.AspNetCore.Mvc;


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

        public CreateProductCommandHandler(IBlobService blobService, IApplicationContext context)
        {
            _blobService = blobService;

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
                ImgId = request.ProductRequest.ImgId,
                ImgIdTwo = request.ProductRequest.ImgIdTwo,
                ImgIdThree = request.ProductRequest.ImgIdThree,
                ImgIdFour = request.ProductRequest.ImgIdFour,
                Size = request.ProductRequest.Size,
            };
            _context.Products.Add(product);
            var res = await _context.SaveChangesAsync(cancellationToken);
            return new OkObjectResult(product);
        }
    }
}
