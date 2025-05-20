using Api.Domain.Models;
using Api.Features.Common.Services.UrlHelper;
using static Api.Features.Orders.Queries.GetAllOrders.GetOrdersQueryHandler;

namespace Api.Features.Orders.Queries.GetAllOrders
{

    public static class GetAllOrdersMapper
    {
        public static OrderResult ToOrderResult(this OrderDto order, IUrlHelpers _urlHelpers)
        {

            return new OrderResult
            {

                /*    Id = product.Id,
                    Title = product.ProductTitle,
                    Description = product.ProductShortDesc*/

                Status = order.Status,
                Price = order.Items.Sum(i => i.TotalProductsPrice),
                OrderItems =  order.Items
  ,
            };

        }
        public static OrderItemResult ToOrderItemResult(this OrderItemDto orderItemDto, IUrlHelpers _urlHelpers)
        {
            return new OrderItemResult
            {
                ProductImageUrl = CreatePictureUrl(orderItemDto.ImageId, _urlHelpers),
                Quantity = orderItemDto.Quantity,
                ProductName = orderItemDto.ProductTitle,
               TotalProductsPrice = orderItemDto.TotalProductsPrice,
               ProductShortDesc = orderItemDto.ProductShortDesc,


            };
        }

      
        private static string CreatePictureUrl(Guid? imgId, IUrlHelpers _urlHelpers)
        {

            if (_urlHelpers == null)
                throw new InvalidOperationException("UrlHelper is not set.");



            var url = _urlHelpers.CreatePictureUrl(imgId);
            if (url == null)
            {
                return "Unable to generate the URL.";
            }


            return string.IsNullOrEmpty(url) ? "Unable to generate the URL." : url;
        }

    }
    class OrderItemFromDb
    {
        public Guid Id { get; set; }


        public string ProductTitle { get; set; }
        public string ProductShortDesc { get; set; }

        public int Quantity { get; set; }
        public float TotalProductsPrice { get; set; }
    }

}
