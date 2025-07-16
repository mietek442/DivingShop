using Api.Shared.Enums;

namespace Api.Domain.Models
{
    public class OrderStatusHistory
    {
        public Guid Id { get; set; }
        public Guid OrderId { get; set; }
        public OrderStatus Status { get; set; }
        public DateTime ChangedAt { get; set; } = DateTime.UtcNow;
    }
}
