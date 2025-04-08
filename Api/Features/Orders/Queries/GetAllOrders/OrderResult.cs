using Api.Domain.Models;
using Api.Shared.Enums;
using static Api.Features.Orders.Queries.GetAllOrders.GetOrdersQueryHandler;

namespace Api.Features.Orders.Queries.GetAllOrders
{
    public class OrderResult
    {
        public List<OrderItemDto> OrderItems { get; set; }
        public OrderStatus Status { get; set; }
        public float Price { get; set; }
    }
}
