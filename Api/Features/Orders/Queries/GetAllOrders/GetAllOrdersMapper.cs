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
                ProductImageUrl = orderItemDto.ImageUrl,
                Quantity = orderItemDto.Quantity,
                ProductName = orderItemDto.ProductTitle,
               TotalProductsPrice = orderItemDto.TotalProductsPrice,
               ProductShortDesc = orderItemDto.ProductShortDesc,


            };
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
