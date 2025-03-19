namespace Api.Features.Orders.Commands.CreateOrder
{
    public class OrderItemRequest
    {
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }    
    }
}
