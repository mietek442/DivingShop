using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Api.Features.Products.Commands.UpdateProduct
{
    public class UpdateProductRequest
    {
        [FromRoute(Name = "id")] public Guid Id { get; set; }
        [FromBody] public required UpdateProductModelRequest UpdateProductModelRequest { get; set; }
    }
}
