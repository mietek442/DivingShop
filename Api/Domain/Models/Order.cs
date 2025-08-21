using Api.Shared.Enums;
using System.ComponentModel.DataAnnotations;

namespace Api.Domain.Models
{
    public class Order
    {
        public Guid Id { get; set; }
        [Required]
        public string? OrderNumber { get; set; } 
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;

        public Guid UserId { get; set; }
        [Required]
        public string? UserName { get; set; }
        public string? UserLastName { get; set; } 
        public string? Address { get; set; } 
        public string? PhoneNumber { get; set; } 
        public string? Email { get; set; } 

        public List<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

       
        public DeliveryMethodEnum DeliveryMethod { get; set; }
        public float Price { get; set; }      
        public float ShipPrice { get; set; }  
        public float TotalPrice { get; set; }

        public float TotalPriceIncludeTax { get; set; }

        // status zamówienia wraz z aktualizacją 
        public OrderStatus Status { get; set; }
        public List<OrderStatusHistory> StatusHistory { get; set; } = new List<OrderStatusHistory>();

        public bool isIsDeleted { get; set; }
        public void ChangeStatus(OrderStatus newStatus)
        {
            if (Status != newStatus)
            {
                Status = newStatus;
                UpdatedAt = DateTime.UtcNow;

                StatusHistory.Add(new OrderStatusHistory
                {
                    OrderId = this.Id,
                    Status = newStatus,
                    ChangedAt = UpdatedAt.Value
                });
            }
        }
    }
}
