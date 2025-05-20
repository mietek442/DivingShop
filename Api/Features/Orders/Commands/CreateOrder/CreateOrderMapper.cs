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
                OrderItems = orderItems,
                Status = order.Status.ToString(),
                Price = orderItems.Sum(i => i.TotalProductsPrice),
            };
        }
    }
}
