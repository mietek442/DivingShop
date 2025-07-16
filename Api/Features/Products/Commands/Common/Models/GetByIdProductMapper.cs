using Api.Domain.Models;
using Api.Features.Common.Services.UrlHelper;

namespace Api.Features.Products.Commands.Common.Models
{
    public static class GetByIdProductMapper
    {
        public static ProductByIdResult ToProductResult(this Product product, IUrlHelpers urlHelpers)
        {
            return new ProductByIdResult
            {
                Title = product.Title,
                ShortDesc = product.ShortDesc,
                Description = product.Description,
                Manufacture = product.Manufacture,
                Available = product.Available,
                BasePrice = product.Discount > 0 ? product.BasePrice : null,
                Discount = product.Discount > 0 ? product.Discount : null,
                FinalPrice = CalculateFinalPrice(product),
                ImageUrls = product.ImageUrls,
                Size = product.Size,
                ProductParams = product.ProductParams
            };
        }

        private static float CalculateFinalPrice(Product product)
        {
            if (product.Discount == 0)
            {
                return product.BasePrice;
            }
            if (product.Discount > 0)
            {
                return product.BasePrice - product.BasePrice * (product.Discount ?? 0);
            }

            return product.BasePrice;
        }

    }
}
