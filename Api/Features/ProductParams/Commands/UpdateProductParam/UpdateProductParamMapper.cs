using Api.Domain.Models;
using Api.Features.ProductParams.Commands.CreateProductParam;

namespace Api.Features.ProductParams.Commands.UpdateProductParam
{
    public class UpdateProductParamMapper
    {
        public static void MapToProductParam(ProductParam productParam, UpdateProductParamModelRequest request)
        {
            productParam.Title = request.Title;
            productParam.Parameter = request.Parameter;
            productParam.Description = request.Description;
            productParam.InfoParam = request.InfoParam;
        }
    }
}
