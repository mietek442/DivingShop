using Api.Domain.Models;
using Api.Features.Common.Services.UrlHelper;
using Api.Infrastructure.DbContext;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;

namespace Api.Features.Products.Queries.GetByIdProduct
{
    public class GetProductByIdQuery : IRequest<ActionResult<ProductResult>>
    {
        public Guid Id { get; set; }
    }

    public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, ActionResult<ProductResult>>
    {
        private readonly IApplicationContext _context;
        private readonly IUrlHelpers _urlHelpers;

        public GetProductByIdQueryHandler(IApplicationContext context, IUrlHelpers urlHelpers)
        {
            _context = context;
            _urlHelpers = urlHelpers;
        }

        public async Task<ActionResult<ProductResult>> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            var product = await _context.Products.FindAsync(request.Id);

            var mapproduct = product.ToProductResult(_urlHelpers);
            if (product == null)
            {
                return new NotFoundResult();
            }

            return new OkObjectResult(product);
        }
    }
}
