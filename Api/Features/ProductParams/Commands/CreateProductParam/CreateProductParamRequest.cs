using Api.Features.Products.Commands.UpdateProduct;
using Microsoft.AspNetCore.Mvc;

namespace Api.Features.ProductParams.Commands.CreateProductParam
{
    public class CreateProductParamRequest
    {
        [FromRoute(Name = "id")]
        public Guid Id { get; set; }

        [FromBody]
        public required CreateProductParamModelRequest CreateProductParamModelRequest { get; set; }
    }

}
