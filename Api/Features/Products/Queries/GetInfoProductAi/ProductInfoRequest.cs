using Api.Features.Products.Commands.UpdateProduct;
using Microsoft.AspNetCore.Mvc;

namespace Api.Features.Products.Queries.GetInfoProductAi
{
    public class ProductInfoRequest
    {
        [FromRoute(Name = "id")] public Guid Id { get; set; }
        [FromBody] public required string Question { get; set; }
    }
}


