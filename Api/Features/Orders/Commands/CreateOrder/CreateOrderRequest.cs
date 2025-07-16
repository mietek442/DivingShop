using Api.Features.Orders.Commands.CreateOrder;
using Api.Shared.Enums;

public class CreateOrderRequest
{
    public string? UserName { get; set; }
    public string? UserLastName { get; set; }
    public string? Address { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Email { get; set; }
    public DeliveryMethodEnum DeliveryMethod { get; set; }
    public List<OrderItemRequest> OrderItems { get; set; } = new();
 
}
