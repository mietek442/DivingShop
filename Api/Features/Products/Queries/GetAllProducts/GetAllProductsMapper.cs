using Api.Domain.Models;
using Api.Features.Common.Services.UrlHelper;
using Microsoft.AspNetCore.Mvc.Routing;

namespace Api.Features.Products.Queries.GetAllProducts
{
    public  static class GetAllProductsMapper
    {
        public static ProductResult ToProductResutl(this Product product, IUrlHelpers _urlHelpers)
        {

            return new ProductResult
            {
                Title = product.Title,
                ShortDesc = product.ShortDesc,
                Description = product.Description,
                Manufacture = product.Manufacture,
                Available = product.Available,
                BasePrice = (product.Discount> 0) ? product.BasePrice : null,
                Discount = (product.Discount > 0) ? product.Discount : null,
                FinalPrice = CalculateFinalPrice(product),
                ImgUrl = CreatePictureUrl(product.ImgId, _urlHelpers),
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
                return product.BasePrice - (product.BasePrice * (product.Discount ?? 0));
            }
            
            return product.BasePrice;
        }
        private static string CreatePictureUrl(Guid? imgId, IUrlHelpers _urlHelpers)
        {

            if (2 == null)
                throw new InvalidOperationException("UrlHelper is not set.");

            

            var url = _urlHelpers.CreatePictureUrl(imgId);
            if (url == null) {
                return "Unable to generate the URL.";
            }


            return string.IsNullOrEmpty(url) ? "Unable to generate the URL." : url;
        }
    }

}
