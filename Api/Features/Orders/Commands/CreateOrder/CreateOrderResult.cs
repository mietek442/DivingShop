using Api.Domain.Models;

namespace Api.Features.Orders.Commands.CreateOrder
{
    public class CreateOrderResult
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string? UserName { get; set; }
        public string? UserLastName { get; set; }
        public string? Address { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }

        public List<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
        public string Status { get; set; }
        public float Price { get; set; }
        public float ShipPrice { get; set; }
        public float TotalPrice { get; set; }
        public float TotalPriceIncludeTax { get; set; }
      
    }
}
