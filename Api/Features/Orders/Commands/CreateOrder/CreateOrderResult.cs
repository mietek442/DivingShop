using Api.Domain.Models;

namespace Api.Features.Orders.Commands.CreateOrder
{
    public class CreateOrderResult
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public List<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
        public string Status { get; set; }
        public float Price { get; set; }
    }
}
