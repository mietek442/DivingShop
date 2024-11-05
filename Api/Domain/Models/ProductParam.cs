using System.ComponentModel.DataAnnotations.Schema;

namespace Api.Domain.Models
{
    public class ProductParam
    {
        public int Id { get; set; }

        public string Title { get; set; }
        public string Parameter { get; set; }
        public string Description { get; set; }
        public string InfoParam { get; set; }

        [ForeignKey("Product")]
        public int ProductId { get; set; }
        public Product Product { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
    }
}
