namespace Api.Features.Orders.Commands.UpdateOrder
{
    public class UpdateOrderItemRequestBody
    {
        public Guid Id { get; set; }
        public int Quantity { get; set; }
    }
}
