using Api.Domain.Models;
using Api.Shared.Enums;
using static Api.Features.Orders.Queries.GetAllOrders.GetOrdersQueryHandler;

namespace Api.Features.Orders.Queries.GetAllOrders
{
    public class OrderResult
    {
        public List<OrderItemDto> OrderItems { get; set; }
        public string OrderNumber { get; set; }
        public string? UserName { get; set; }
        public string? UserLastName { get; set; }
        public string? Address { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public DeliveryMethodEnum DeliveryMethod { get; set; }

        public OrderStatus Status { get; set; }

        public float Price { get; set; }
        public float ShipPrice { get; set; }
        public float TotalPrice { get; set; }
        public float TotalPriceIncludeTax { get; set; }
    }
}
