using Api.Features.Common.Services.Storage;
using Api.Features.Common.Services.UrlHelper;
using Api.Features.Products.Queries.GetAllProducts;
using Api.Infrastructure.DbContext;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.EntityFrameworkCore;

namespace Api.Features.Products.Queries.GetProducts
{
    public class GetProductsQuery : IRequest<ActionResult<List<ProductResult>>>
    {
    }

    public class GetProductsQueryHandler : IRequestHandler<GetProductsQuery, ActionResult<List<ProductResult>>>
    {
        private readonly IApplicationContext _context;
       

        private readonly IUrlHelpers _urlHelpers;
        public GetProductsQueryHandler(IApplicationContext context, IUrlHelpers urlHelpers)
        {
            _context = context;
            _urlHelpers = urlHelpers;

        }

        public async Task<ActionResult<List<ProductResult>>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
        {
            var products = await _context.Products
                .Include(p => p.ProductParams)
                .ToListAsync(cancellationToken);

            if (products == null || products.Count == 0)
            {
                return new NotFoundResult();
            }

            var productResponses = products.Select(product => product.ToProductResutl(_urlHelpers)).ToList();
            return new OkObjectResult(productResponses);
        }
    }
}
