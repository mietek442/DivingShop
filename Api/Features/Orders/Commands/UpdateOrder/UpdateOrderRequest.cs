using Api.Shared.Enums;
using Microsoft.AspNetCore.Mvc;

namespace Api.Features.Orders.Commands.UpdateOrder
{
    public class UpdateOrderRequest
    {
        [FromRoute(Name = "id")] public Guid Id { get; set; }
        [FromBody] public required UpdateOrderRequestBody UpdateOrderRequestBody { get; set; }
    }
}
