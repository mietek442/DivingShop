using Api.Domain.Models;

namespace Api.Features.Orders.Queries.GetAllOrders
{
    public class OrderItemResult
    {
        
        

        public string ProductName { get; set; }

        public string ProductImageUrl { get; set; }
        public string ProductShortDesc { get; set; }

        public int Quantity { get; set; }
        public float TotalProductsPrice { get; set; }
    }
}
