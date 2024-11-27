using Api.Domain.Models;
using Api.Features.Common.Services.UrlHelper;
using Api.Features.Products.Commands.Common.Models;
using Api.Infrastructure.DbContext;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;

namespace Api.Features.Products.Queries.GetByIdProduct
{
    public class GetProductByIdQuery : IRequest<ActionResult<ProductByIdResult>>
    {
        public Guid Id { get; set; }
    }

    public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, ActionResult<ProductByIdResult>>
    {
        private readonly IApplicationContext _context;
        private readonly IUrlHelpers _urlHelpers;

        public GetProductByIdQueryHandler(IApplicationContext context, IUrlHelpers urlHelpers)
        {
            _context = context;
            _urlHelpers = urlHelpers;
        }

        public async Task<ActionResult<ProductByIdResult>> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            var product = await _context.Products.FindAsync(request.Id);
            if (product == null)
            {
                return new NotFoundResult();
            }
            var mapproduct = product.ToProductResult(_urlHelpers);
           

            return new OkObjectResult(mapproduct);
        }
    }
}
