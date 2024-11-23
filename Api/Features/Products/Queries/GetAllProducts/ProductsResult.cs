using Api.Domain.Models;

namespace Api.Features.Products.Queries.GetAllProducts
{
    public class ProductResult
    {
     

        public required string Title { get; set; }
        public string ShortDesc { get; set; }
        public string? Description { get; set; }
        public string Manufacture { get; set; }
        public bool Available { get; set; }
        public float? BasePrice { get; set; }
        public float? Discount { get; set; }
        public float FinalPrice { get; set; }
        public string ImgUrl { get; set; }
        
        public float? Size { get; set; }
       

        
    }
}
