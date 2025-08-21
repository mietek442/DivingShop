using Api.Domain.Models;
using Api.Shared.Enums;
using System.ComponentModel.DataAnnotations;

namespace Api.Features.Orders.Commands.UpdateOrder
{
    public class UpdateOrderRequestBody
    {



        public string? UserName { get; set; }
        public string? UserLastName { get; set; }
        public string? Address { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public DeliveryMethodEnum DeliveryMethod { get; set; }
        public List<UpdateOrderItemRequestBody> OrderItems { get; set; } = new();

    }
}
