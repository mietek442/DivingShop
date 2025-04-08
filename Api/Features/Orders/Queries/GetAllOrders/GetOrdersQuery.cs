using Api.Features.Common.Services.UrlHelper;
using Api.Infrastructure.DbContext;
using Api.Shared.Enums;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using static System.Net.Mime.MediaTypeNames;

namespace Api.Features.Orders.Queries.GetAllOrders
{
    public class GetOrdersQuery : IRequest<ActionResult<List<OrderResult>>>
    {
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
            var orders = await _context.Orders
    .Select(o => new OrderDto
    {
        Id = o.Id,
        CreatedAt = o.CreatedAt,
        Status = o.Status,
        Items = o.OrderItems.Select(oi => new OrderItemDto
        {
            Id = oi.Id,
            Quantity = oi.Quantity,
            ProductTitle = oi.Product.Title,
            ProductShortDesc = oi.Product.ShortDesc,
            TotalProductsPrice = oi.TotalProductsPrice,
            ImageId = oi.Product.ImgId,
        }).ToList()
    })
    .ToListAsync(cancellationToken);


            var mappedOrders = orders.Select(o => o.ToOrderResult(_urlHelpers)).ToList();

            /*
                        if (orders == null || orders.Count == 0)
                        {
                            return new NotFoundResult();
                        }

                        var orderResponses = orders.Select(order => order.ToOrderResult(_urlHelpers)).ToList(); */
            return new OkObjectResult(orders);
        }
        public class OrderDto
        {
            public Guid Id { get; set; }
            public OrderStatus Status { get; set; }
            public DateTime CreatedAt { get; set; }
            public List<OrderItemDto> Items { get; set; }
        }

        public class OrderItemDto
        {
            public Guid Id { get; set; }
            public int Quantity { get; set; }
            public string ProductTitle { get; set; }
            public string ProductShortDesc { get; set; }
            public Guid? ImageId { get; set; }

            public float TotalProductsPrice { get; set; }
        }
    }
}
