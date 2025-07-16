using Api.Domain.Models;
using Api.Features.Common.Services.UrlHelper;
using Api.Features.Products.Queries.GetProducts;
using Api.Infrastructure.DbContext;
using Api.Shared.Enums;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Linq.Expressions;
using static System.Net.Mime.MediaTypeNames;

namespace Api.Features.Orders.Queries.GetAllOrders
{
    public class GetOrdersQuery : IRequest<ActionResult<List<OrderResult>>>
    {
        public required OrderQueryOptionsRequest QueryOptionsObject { get; set; }
    }

    public class GetOrdersQueryHandler : IRequestHandler<GetOrdersQuery, ActionResult<List<OrderResult>>>
    {
        private readonly IApplicationContext _context;
        private readonly IUrlHelpers _urlHelpers;

        public GetOrdersQueryHandler(IApplicationContext context, IUrlHelpers urlHelpers)
        {
            _context = context;
            _urlHelpers = urlHelpers;
        }

        public async Task<ActionResult<List<OrderResult>>> Handle(GetOrdersQuery request, CancellationToken cancellationToken)
        {




            var pageNumber = request.QueryOptionsObject.PageNumber;
            var pageSize = request.QueryOptionsObject.PageSize;
            int skip = (pageNumber - 1) * pageSize;


            IQueryable<OrderDto> ordersQuery = _context.Orders
           .OrderBy(o => o.CreatedAt)
           .Select(o => new OrderDto
           {
               Id = o.Id,
               CreatedAt = o.CreatedAt,
               Status = o.Status,
               UserName = o.UserName,
               UserLastName = o.UserLastName,
               Address = o.Address,
               OrderNumber=o.OrderNumber,
               PhoneNumber = o.PhoneNumber,
               Email = o.Email,
              
               Price = o.Price,
               ShipPrice = o.ShipPrice,
               TotalPrice = o.TotalPrice,
               TotalPriceIncludeTax = o.TotalPriceIncludeTax,
               Items = o.OrderItems.Select(oi => new OrderItemDto
               {
                   Id = oi.Id,
                   Quantity = oi.Quantity,
                   ProductTitle = oi.Product.Title,
                   ProductShortDesc = oi.Product.ShortDesc,
                   TotalProductsPrice = oi.TotalProductsPrice,
                   ImageUrl =  oi.Product.ImageUrls.Count > 0 ? oi.Product.ImageUrls[0] : null,
               }).ToList()
           });

            if (request.QueryOptionsObject.IsDescSort)
            {
                ordersQuery = ordersQuery.OrderByDescending(GetSortProperty(request.QueryOptionsObject));
            }
            else
            {
                ordersQuery = ordersQuery.OrderBy(GetSortProperty(request.QueryOptionsObject));
            }

            var orders = await ordersQuery
            .Skip(skip)
            .Take(pageSize)
            .ToListAsync(cancellationToken);


            var mappedOrders = orders.Select(o => o.ToOrderResult(_urlHelpers)).ToList();


            return new OkObjectResult(orders);
        }

        //sortowanie 
        private static Expression<Func<OrderDto, object>> GetSortProperty(OrderQueryOptionsRequest request) =>
     request.SortBy?.ToString().ToLower() switch
     {
         "createdate" => order => order.CreatedAt,
         "status" => order => order.Status,
         _ => order => order.Id
     };
        public class OrderDto
        {
            public Guid Id { get; set; }
            
            public OrderStatus Status { get; set; }
            public DateTime CreatedAt { get; set; }
            public List<OrderItemDto> Items { get; set; }
            required
            public string OrderNumber { get; set; }
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

        public class OrderItemDto
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
