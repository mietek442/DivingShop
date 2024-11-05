namespace Api.Domain.Models
{
    public class Product
    {
        public int Id { get; set; }

        public required string Title { get; set; }
        public string ShortDesc { get; set; }
        public required string Description { get; set; }
        public string Manufacture { get; set; }
        public bool Available { get; set; }
        public float? BasePrice { get; set; }
        public float? Discount { get; set; }
        public Guid? ImgId { get; set; }
        public Guid? ImgIdTwo { get; set; }
        public Guid? ImgIdThree { get; set; }
        public Guid? ImgIdFour { get; set; }
        public float? Size { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<ProductParam> ProductParams { get; set; }
    }
}
