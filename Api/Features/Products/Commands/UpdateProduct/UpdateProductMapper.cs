using Api.Domain.Models;
using Api.Features.Common.Services.UrlHelper;
using Api.Features.Products.Queries.GetByIdProduct;

namespace Api.Features.Products.Commands.UpdateProduct
{
    public static class UpdateProductMapper
    {
        public static void MapToProduct(Product product, UpdateProductModelRequest request)
        {
            product.Title = request.Title;
            product.ShortDesc = request.ShortDesc;
            product.Description = request.Description;
            product.Manufacture = request.Manufacture;
            product.Available = request.Available;
            product.BasePrice = request.BasePrice;
            product.Discount = request.Discount;
            product.ImageUrls = request.ImageUrls;
            product.Size = request.Size;
        }
    }
}

