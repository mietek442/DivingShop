using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Api.Features.Products.Commands.CreateProduct
{
    public class CreateProductRequest
    {
        public List<IFormFile> Images { get; set; }
        [Required]
        
        public string Title { get; set; }

        [Required]

        public string ShortDesc { get; set; }

        public string Description { get; set; }

        public string Manufacture { get; set; }

        [Required]
        public bool Available { get; set; }

        [Required]
        public float BasePrice { get; set; }

        public float? Discount { get; set; }

        [Required]


        public float? Size { get; set; }
    }
}
