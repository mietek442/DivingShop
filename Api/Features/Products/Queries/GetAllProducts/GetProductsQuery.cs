using Api.Domain.Models;
using Api.Features.Common.Services.Storage;
using Api.Features.Common.Services.UrlHelper;
using Api.Features.Products.Queries.GetAllProducts;
using Api.Infrastructure.DbContext;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Api.Features.Products.Queries.GetProducts
{
    public class GetProductsQuery : IRequest<ActionResult<List<ProductResult>>>
    {
    }

    public class GetProductsQueryHandler : IRequestHandler<GetProductsQuery, ActionResult<List<ProductResult>>>
    {
        private readonly IApplicationContext _context;
        //GetProductsPaginateQuery getProductsPaginateQuery { get; set; }

        private readonly IUrlHelpers _urlHelpers;
        public GetProductsQueryHandler(IApplicationContext context, IUrlHelpers urlHelpers, GetProductsPaginateQuery getProductsPaginateQuery)
        {
            _context = context;
            _urlHelpers = urlHelpers;
             
        }

        public async Task<ActionResult<List<ProductResult>>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
        {
            IQueryable<Product> productsQuery = _context.Products;


           // Console.WriteLine(getProductsPaginateQuery.SearchTitle);
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
        private static Expression<Func<Product, object>> GetSortProperty(GetProductsPaginateQuery request) =>
    request.SortColumn?.ToLower() switch
    {
        "title" => static product => product.Title,
     
     
        _ => static product => product.Id
    };

    }

}
