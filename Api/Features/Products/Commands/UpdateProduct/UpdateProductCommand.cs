using Api.Features.Common.Services.UrlHelper;
using Api.Features.Products.Commands.Common.Models;
using Api.Infrastructure.DbContext;
using MediatR;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc;

namespace Api.Features.Products.Commands.UpdateProduct
{
    public class UpdateProductCommand: IRequest<ActionResult<ProductByIdResult>>
    {
        public Guid Id { get; set; }
        public required UpdateProductModelRequest UpdateProductModelRequest { get; set; }
    }
    public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, ActionResult<ProductByIdResult>>
    {
        private readonly IApplicationContext _context;
        private readonly IUrlHelpers _urlHelpers;
        private readonly IMediator _mediator;
        public UpdateProductCommandHandler(IApplicationContext context, IUrlHelpers urlHelpers, IMediator mediator)
        {
            _context = context;
            _urlHelpers = urlHelpers;
            _mediator = mediator;   
        }
        public async Task<ActionResult<ProductByIdResult>> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            var product = await _context.Products.FindAsync(request.Id, cancellationToken);
            if (product == null)
            {
                return new NotFoundResult();
            }
            //Mapowanie rzeczy z request do produktu np. product.title = request.UpdateProductModelRequest.title
            UpdateProductMapper.MapToProduct(product, request.UpdateProductModelRequest);

            
            await _context.SaveChangesAsync(cancellationToken);


            //zwracanie Product By ID Result
            return product.ToProductResult(_urlHelpers);

        }
    }
}
