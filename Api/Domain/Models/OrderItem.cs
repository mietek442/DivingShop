namespace Api.Domain.Models
{
    public class OrderItem
    {
        public Guid Id { get; set; }
        public Guid OrderId { get; set; }

        public Guid ProductId { get; set; }
        public Product Product { get; set; }

        public string ProductName { get; set; }

        public int Quantity { get; set; }
        public float TotalProductsPrice { get; set; }
    }
}
