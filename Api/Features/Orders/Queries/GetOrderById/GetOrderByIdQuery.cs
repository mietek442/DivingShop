using Api.Domain.Models;
using Api.Features.Common.Services.UrlHelper;
using Api.Features.Orders.Queries.GetAllOrders;
using Api.Infrastructure.DbContext;
using Api.Shared.Enums;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static Api.Features.Orders.Queries.GetOrderById.GetOrderByIdQueryHandler;

namespace Api.Features.Orders.Queries.GetOrderById
{
    public class GetOrderByIdQuery : IRequest<ActionResult<OrderByIDto>>
    {
        public required Guid OrderId { get; set; }
    }

    public class GetOrderByIdQueryHandler : IRequestHandler<GetOrderByIdQuery, ActionResult<OrderByIDto>>
    {
        private readonly IApplicationContext _context;
        private readonly IUrlHelpers _urlHelpers;

        public GetOrderByIdQueryHandler(IApplicationContext context, IUrlHelpers urlHelpers)
        {
            _context = context;
            _urlHelpers = urlHelpers;
        }

        public async Task<ActionResult<OrderByIDto>> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
        {
            var order = await _context.Orders
                .Where(o => o.Id == request.OrderId)
                .Select(o => new OrderByIDto
                {
                   Id = o.Id,
                    CreatedAt = o.CreatedAt,
                    Status = o.Status,
                    UserName = o.UserName,
                    UserLastName = o.UserLastName,
                    Address = o.Address,
                    PhoneNumber = o.PhoneNumber,
                    Email = o.Email,
                    OrderNumber = o.OrderNumber,
                    Price = o.Price,
                    ShipPrice = o.ShipPrice,
                    DeliveryMethod=o.DeliveryMethod,
                    TotalPrice = o.TotalPrice,
                    TotalPriceIncludeTax = o.TotalPriceIncludeTax,
                    Items = o.OrderItems.Select(oi => new OrderByIDItemDto
                    {
                        Id = oi.Id,
                        Quantity = oi.Quantity,
                        ProductTitle = oi.Product.Title,
                        ProductShortDesc = oi.Product.ShortDesc,
                        TotalProductsPrice = oi.TotalProductsPrice,
                        ImageUrl = oi.Product.ImageUrls.Count > 0 ? oi.Product.ImageUrls[0] : null,
                    }).ToList()
                })
                .FirstOrDefaultAsync(cancellationToken);

            if (order == null)
                return new NotFoundResult();

           
            return new OkObjectResult(order);
        }

        public class OrderByIDto
        {
            public Guid Id { get; set; }
            public OrderStatus Status { get; set; }
            public DateTime CreatedAt { get; set; }
            public List<OrderByIDItemDto> Items { get; set; }
            public required string OrderNumber { get; set; }
            public DeliveryMethodEnum DeliveryMethod { get; set; }
            public string? UserName { get; set; }
            public string? UserLastName { get; set; }
            public string? Address { get; set; }
            public string? PhoneNumber { get; set; }
            public string? Email { get; set; }

            public float Price { get; set; }
            public float ShipPrice { get; set; }
            public float TotalPrice { get; set; }
            public float TotalPriceIncludeTax { get; set; }
        }

        public class OrderByIDItemDto
        {
            public Guid Id { get; set; }
            public int Quantity { get; set; }
            public string ProductTitle { get; set; }
            public string ProductShortDesc { get; set; }
            public string? ImageUrl { get; set; }
            public float TotalProductsPrice { get; set; }
        }
    }
}
