using Microsoft.EntityFrameworkCore;

namespace Api.Features.Products.Queries.GetAllProducts
{
    public class PagedProductList<T>
    {
        private PagedProductList(List<T> items, int page, int pageSize, int totalCount)
        {
            Items = items;
            Page = page;
            PageSize = pageSize;
            TotalCount = totalCount;
        }

        public List<T> Items { get; }

        public int Page { get; }

        public int PageSize { get; }

        public int TotalCount { get; }

        public bool HasNextPage => (Page * PageSize) < TotalCount;

        public bool HasPreviousPage => Page > 1;

        public static async Task<PagedProductList<T>> CreateAsync(IQueryable<T> source, int page, int pageSize)
        {
            var totalCount = await source.CountAsync();
            var items = await source.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            return new PagedProductList<T>(items, page, pageSize, totalCount);
        }
    }
}
