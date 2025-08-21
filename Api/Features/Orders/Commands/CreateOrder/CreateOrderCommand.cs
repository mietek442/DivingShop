using Api.Domain.Models;
using Api.Infrastructure.DbContext;
using Api.Shared.Enums;
using Azure.Core;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

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
                var order = CreateNewOrder(loggedInUserId,request.OrderRequest); // tworzymy Order z podstawowymi ifnormacjami

                await _context.Orders.AddAsync(order, cancellationToken); // dodajemy do baz(ale możemy zawsze to cofnąć przez rolback-który jest nizej jak sie cos nie uda)

                var productIds = request.OrderRequest.OrderItems.Select(i => i.ProductId).Distinct(); // lista id produktów które dodaliśmy
                var products = await _context.Products
                    .Where(p => productIds.Contains(p.Id))
                    .ToDictionaryAsync(p => p.Id, cancellationToken);

                var orderItems = request.OrderRequest.OrderItems
                    .Select(itemRequest => CreateOrderItem(itemRequest, order.Id, products[itemRequest.ProductId]))
                    .ToList();


                CalculateFinalOrderPrice(order, orderItems); // w zamówieniu edycja cenn 




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

        private Order CreateNewOrder(Guid userId, CreateOrderRequest request)
        {
            return new Order
            {
                UserId = userId,
                UserName = request.UserName,
                UserLastName = request.UserLastName,
                OrderNumber = Guid.NewGuid().ToString()[..8], 
                Address = request.Address,
                PhoneNumber=request.PhoneNumber,
                Email=request.Email,
                OrderDate = AddBusinessDays(DateTime.UtcNow, 2),// czas dostawy ustawiony na 2 dni robocze
                DeliveryMethod =request.DeliveryMethod,

                Status = OrderStatus.Pending,
                CreatedAt = DateTime.UtcNow
            };
        }

        private OrderItem CreateOrderItem(OrderItemRequest itemRequest, Guid orderId, Product product)
        {
            return new OrderItem
            {
                OrderId = orderId,
                ProductId = itemRequest.ProductId,
                ProductName = product.Title,
                Quantity = itemRequest.Quantity,
                TotalProductsPrice = itemRequest.Quantity * CalculateFinalPrice(product),
            };
        }

        private static float CalculateFinalPrice(Product product)
        {
            return product.BasePrice * (1 - (product.Discount ?? 0));
        }
        private void CalculateFinalOrderPrice(Order order, List<OrderItem> orderItems)
        {
            

            var itemsPrice = orderItems.Sum(i => i.TotalProductsPrice);
            order.Price = (float)Math.Round(itemsPrice, 2);

            order.ShipPrice = order.DeliveryMethod switch
            {
                DeliveryMethodEnum.Standard => 13.99f,
                DeliveryMethodEnum.Express => 19.99f,
                DeliveryMethodEnum.PickupPoint => 11.99f,
                DeliveryMethodEnum.Courier => 15.99f,
                _ => 10f
            };

            order.ShipPrice = (float)Math.Round(order.ShipPrice, 2);

            order.TotalPrice = (float)Math.Round(order.Price + order.ShipPrice, 2);
            order.TotalPriceIncludeTax = (float)Math.Round(order.TotalPrice * 1.24f, 2);
        }
        public static DateTime AddBusinessDays(DateTime startDate, int businessDays)
        {
            var currentDate = startDate;
            while (businessDays > 0)
            {
                currentDate = currentDate.AddDays(1);
                if (currentDate.DayOfWeek != DayOfWeek.Saturday && currentDate.DayOfWeek != DayOfWeek.Sunday)
                {
                    businessDays--;
                }
            }
            return currentDate;
        }


    }

}