using Api.Domain.Models;
using Api.Features.Common.Services.UrlHelper;
using Api.Features.ProductParams.Commands.CreateProductParam;
using Api.Features.Products.Queries.GetByIdProduct;

namespace Api.Features.Products.Commands.UpdateProduct
{
    public static class CreateProductParamMapper
    {
        public static void MapToProductParam(ProductParam productParam, CreateProductParamModelRequest request,Guid ProductId)
        {
            productParam.ProductId = ProductId;
            productParam.Title = request.Title;
            productParam.Parameter = request.Parameter;
            productParam.Description = request.Description;
            productParam.InfoParam = request.InfoParam;
            
        }
    }
}
