using Api.Domain.Models;
using Api.Infrastructure.DbContext;
using Api.Shared.Enums;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api.Features.Orders.Commands.CreateOrder
{
    public class CreateOrderCommand : IRequest<ActionResult<CreateOrderResult>>
    {
        public CreateOrderRequest OrderRequest { get; set; }
    }

    public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, ActionResult<CreateOrderResult>>
    {
        private readonly IApplicationContext _context;

        public CreateOrderCommandHandler(IApplicationContext context)
        {
            _context = context;
        }

        public async Task<ActionResult<CreateOrderResult>> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            var loggedInUserId = new Guid();


            // transation działa tak jak wszystkie produkty są git to dodamy wsyzstko do bazy(zamówienie i itemy) a jak jest jakiś bład to nie ma nic w bazie
            using var transaction = await _context.BeginTransactionAsync(cancellationToken);
            try
            {
                var order = CreateNewOrder(loggedInUserId);
                await _context.Orders.AddAsync(order, cancellationToken);

                var productIds = request.OrderRequest.OrderItems.Select(i => i.ProductId).Distinct();
                var products = await _context.Products
                    .Where(p => productIds.Contains(p.Id))
                    .ToDictionaryAsync(p => p.Id, cancellationToken);

                var orderItems = request.OrderRequest.OrderItems
                    .Select(itemRequest => CreateOrderItem(itemRequest, order.Id, products[itemRequest.ProductId]))
                    .ToList();

                await _context.OrderItems.AddRangeAsync(orderItems, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);

                await transaction.CommitAsync(cancellationToken);

                var mappedOrders = order.ToCreateOrderResult(orderItems);
                return new OkObjectResult(mappedOrders);
            }
            catch
            {
                await transaction.RollbackAsync(cancellationToken);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        private Order CreateNewOrder(Guid userId)
        {
            return new Order
            {
                UserId = userId,
                Status = OrderStatus.Pending,
                CreatedAt = DateTime.UtcNow
            };
        }

        private OrderItem CreateOrderItem(OrderItemRequest itemRequest, Guid orderId,Product product)
        {
            return new OrderItem
            {
                OrderId = orderId,
                ProductId = itemRequest.ProductId,
                ProductName = product.Title,
                Quantity = itemRequest.Quantity,
                TotalProductsPrice = itemRequest.Quantity* CalculateFinalPrice(product),
            };
        }
     
        private static float CalculateFinalPrice(Product product)
        {
            return product.BasePrice * (1 - (product.Discount ?? 0));
        }

    }

}

