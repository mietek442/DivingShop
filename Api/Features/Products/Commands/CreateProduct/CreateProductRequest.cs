using System.ComponentModel.DataAnnotations;

namespace Api.Features.Products.Commands.CreateProduct
{
    public class CreateProductRequest
    {
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
        public Guid ImgId { get; set; }

        public Guid? ImgIdTwo { get; set; }
        public Guid? ImgIdThree { get; set; }
        public Guid? ImgIdFour { get; set; }

        public float? Size { get; set; }
    }
}
