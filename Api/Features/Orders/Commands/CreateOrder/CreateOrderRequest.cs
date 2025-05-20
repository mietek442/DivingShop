namespace Api.Features.Orders.Commands.CreateOrder
{
    public class CreateOrderRequest
    {
        public List<OrderItemRequest> OrderItems { get; set; } = new List<OrderItemRequest>();
    }
}
