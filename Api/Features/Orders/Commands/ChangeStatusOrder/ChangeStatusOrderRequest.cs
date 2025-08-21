using Api.Features.Products.Commands.UpdateProduct;
using Api.Shared.Enums;
using Microsoft.AspNetCore.Mvc;

namespace Api.Features.Orders.Commands.ChangeStatusOrder
{
    public class ChangeStatusOrderRequest
    {
        [FromRoute(Name = "id")] public Guid Id { get; set; }
        [FromBody] public required OrderStatus OrderStatus { get; set; }

    }
}
