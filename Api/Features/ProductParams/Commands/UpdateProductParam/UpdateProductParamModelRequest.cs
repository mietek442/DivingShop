using Api.Domain.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace Api.Features.ProductParams.Commands.UpdateProductParam
{
    public class UpdateProductParamModelRequest
    {
       

        public string Title { get; set; }
        public string Parameter { get; set; }
        public string Description { get; set; }
        public string InfoParam { get; set; }


    }
}
