using Api.Features.ProductParams.Commands.CreateProductParam;
using Api.Features.Products.Commands.UpdateProduct;
using Microsoft.AspNetCore.Mvc;

namespace Api.Features.ProductParams.Commands.UpdateProductParam
{
    public class UpdateProductParamRequest
    {
        [FromRoute(Name = "id")]
        public Guid Id { get; set; }

        [FromBody]
        public required UpdateProductParamModelRequest UpdateProductParamModelRequest { get; set; }
    }
}
