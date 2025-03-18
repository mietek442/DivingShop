using Api.Domain.Models;
using Api.Features.Common.Services.Storage;
using Api.Features.Products.Commands.UpdateProduct;
using Api.Infrastructure.DbContext;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Features.ProductParams.Commands.CreateProductParam
{
    public class CreateProductParamCommand : IRequest<ActionResult<ProductParam>>
    {
        public Guid ProductId { get; set; }
        public CreateProductParamModelRequest ProductParamRequest { get; set; }
    }

    public class CreateProductParamCommandHandler : IRequestHandler<CreateProductParamCommand, ActionResult<ProductParam>>
    {

        private readonly IApplicationContext _context;

        public CreateProductParamCommandHandler(IBlobService blobService, IApplicationContext context)
        {
          
            _context = context;
        }

        public async Task<ActionResult<ProductParam>> Handle(CreateProductParamCommand request, CancellationToken cancellationToken)
        {
            var product = await _context.Products.FindAsync(request.ProductId);
            if (product == null)
            {
                return new NotFoundResult();
            }
          
            var productParam = new ProductParam();
            CreateProductParamMapper.MapToProductParam(productParam,request.ProductParamRequest, request.ProductId);


            _context.ProductParams.Add(productParam);
            var res = await _context.SaveChangesAsync(cancellationToken);
            return new OkObjectResult(productParam);
        }
    }
}
