using Api.Domain.Models;
using Api.Features.Common.Services.UrlHelper;
using Api.Infrastructure.DbContext;
using Api.Shared.Enums;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Api.Features.Orders.Commands.UpdateOrder;

public sealed class UpdateOrderCommand : IRequest<ActionResult>
{
    public Guid OrderId { get; set; }
    public UpdateOrderRequestBody UpdateOrderRequestBody { get; set; }
}

public sealed class UpdateOrderCommandHandler : IRequestHandler<UpdateOrderCommand, ActionResult>
{
    private readonly IApplicationContext _context;

    public UpdateOrderCommandHandler(IApplicationContext context)
    {
        _context = context;
    }

    public async Task<ActionResult> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
    {
        var order = await _context.Orders
            .Include(o => o.OrderItems)
            .FirstOrDefaultAsync(o => o.Id == request.OrderId, cancellationToken);

        if (order is null)
        {
            return new OkResult();
        }

        UpdateOrderMapper.MapToOrder(order, request.UpdateOrderRequestBody);
        UpdateOrderItems(order, request.UpdateOrderRequestBody);
        CalculateFinalOrderPrice(order, order.OrderItems);
        await _context.SaveChangesAsync(cancellationToken);

        return new OkResult();
    }
    private void CalculateFinalOrderPrice(Order order, List<OrderItem> orderItems)
    {


        var itemsPrice = orderItems.Sum(i => i.TotalProductsPrice);
        order.Price = (float)Math.Round(itemsPrice,2);

        order.ShipPrice = order.DeliveryMethod switch
        {
            DeliveryMethodEnum.Standard => 13.99f,
            DeliveryMethodEnum.Express => 19.99f,
            DeliveryMethodEnum.PickupPoint => 11.99f,
            DeliveryMethodEnum.Courier => 15.99f,
            _ => 10f
        };

        order.TotalPrice = (float)Math.Round(itemsPrice + order.ShipPrice,2);
        order.TotalPriceIncludeTax = (float)Math.Round(order.TotalPrice * 1.24f,2);
    }
    private void UpdateOrderItems(Order order, UpdateOrderRequestBody requestBody)
    {
        foreach (var item in requestBody.OrderItems)
        {
            var existingItem = order.OrderItems.FirstOrDefault(i => i.Id == item.Id);
            if (existingItem is null)
                continue;

            if (item.Quantity == 0)
            {
                _context.OrderItems.Remove(existingItem);
                order.OrderItems.Remove(existingItem);
            }
            else
            {
                var pricePerOne =   existingItem.TotalProductsPrice/existingItem.Quantity;
                
                existingItem.Quantity = item.Quantity;
                existingItem.TotalProductsPrice = (float) Math.Round( pricePerOne * item.Quantity,2);
               
            }
        }
    }
}
