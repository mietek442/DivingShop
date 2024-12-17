
using MediatR;

namespace Api.Features.Products.Queries.GetAllProducts
{
    public record GetProductsPaginateQuery(
        string? SearchTitle,
        string? SortColumn,
        string? SortOrder,
        int Page,
          int PageSize) : IRequest<PagedProductList<ProductResult>>;
}
