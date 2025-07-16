using Api.Domain.Models;

namespace Api.Features.Products.Commands.Common.Models
{
    public class ProductByIdResult
    {

        public required string Title { get; set; }
        public string ShortDesc { get; set; }
        public string? Description { get; set; }
        public string Manufacture { get; set; }
        public bool Available { get; set; }
        public float? BasePrice { get; set; }
        public float? Discount { get; set; }
        public float FinalPrice { get; set; }
        public List<string> ImageUrls { get; set; } = new();
        public float? Size { get; set; }
        public ICollection<ProductParam>? ProductParams { get; set; }

    }
}
