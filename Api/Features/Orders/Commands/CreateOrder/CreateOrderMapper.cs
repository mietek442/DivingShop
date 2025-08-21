using Api.Domain.Models;

namespace Api.Features.Orders.Commands.CreateOrder
{
    public static class CreateOrderMapper
    {
        public static CreateOrderResult ToCreateOrderResult(this Order order, List<OrderItem> orderItems)
        {
            return new CreateOrderResult
            {
                Id = order.Id,
                UserId = order.UserId,
                UserName=order.UserName,
                UserLastName=order.UserLastName,
                Address=order.Address,
                Email=order.Email,
                PhoneNumber=order.PhoneNumber,
                OrderItems = orderItems,
                DeliveryMethod = order.DeliveryMethod,
                Status = order.Status.ToString(),
                Price = order.Price,
                ShipPrice=order.ShipPrice,
                TotalPrice=order.TotalPrice,
                TotalPriceIncludeTax =order.TotalPriceIncludeTax
            };
        }
    }
}
