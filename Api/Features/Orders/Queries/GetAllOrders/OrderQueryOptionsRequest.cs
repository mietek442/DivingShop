using Api.Shared.Enums;

namespace Api.Features.Orders.Queries.GetAllOrders
{
    public class OrderQueryOptionsRequest
    {
        public string? SortBy { get; set; } = null;
        public bool IsDescSort { get; set; } = false;
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 5;

        public OrderStatus? OrderStatus { get; set; }
        public DateTime? CreatedAt { get; set; }
        public Guid? UserId { get; set; }
        public bool? IsDeleted { get; set; }
    }
}
