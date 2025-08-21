using Api.Domain.Models;
using Api.Features.Products.Commands.UpdateProduct;

namespace Api.Features.Orders.Commands.UpdateOrder
{
    public static class UpdateOrderMapper
    {
        public static void MapToOrder(Order order, UpdateOrderRequestBody request)
        {
            order.UserName = request.UserName;
            order.UserLastName = request.UserLastName;
            order.Address = request.Address;
            order.PhoneNumber = request.PhoneNumber;
            order.Email = request.Email;
            order.DeliveryMethod = request.DeliveryMethod;
          

        }
    }
}
