using Api.Domain.Models;
using Api.Features.Common.Services.UrlHelper;

namespace Api.Features.Products.Queries.GetByIdProduct
{
    public static class GetByIdProductMapper
    {
        public static ProductResult ToProductResult(this Product product, IUrlHelpers urlHelpers)
        {
            return new ProductResult
            {
                Title = product.Title,
                ShortDesc = product.ShortDesc,
                Description = product.Description,
                Manufacture = product.Manufacture,
                Available = product.Available,
                BasePrice = (product.Discount > 0) ? product.BasePrice : null,
                Discount = (product.Discount > 0) ? product.Discount : null,
                FinalPrice = CalculateFinalPrice(product),
                ImgUrl = CreatePictureUrl(product.ImgId, urlHelpers),
                ImgTwo = CreatePictureUrl(product.ImgIdTwo, urlHelpers),
                ImgThree = CreatePictureUrl(product.ImgIdThree, urlHelpers),
                ImgFour = CreatePictureUrl(product.ImgIdFour, urlHelpers),
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
                return product.BasePrice - (product.BasePrice * (product.Discount ?? 0));
            }

            return product.BasePrice;
        }

        private static string CreatePictureUrl(Guid? imgId, IUrlHelpers urlHelpers)
        {

            if (urlHelpers == null)
                throw new InvalidOperationException("UrlHelper is not set.");



            var url = urlHelpers.CreatePictureUrl(imgId);
            if (url == null)
            {
                return "Unable to generate the URL.";
            }


            return string.IsNullOrEmpty(url) ? "Unable to generate the URL." : url;
        }
    }
}
