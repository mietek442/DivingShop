using Api.Features.Common.Services.UrlHelper;
using Api.Infrastructure.DbContext;
using Api.Shared.Enums;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Api.Features.Orders.Commands.ChangeStatusOrder
{
    public class UpdateOrderStatusCommand : IRequest<ActionResult>
    {
        public Guid OrderId { get; set; }
        public required OrderStatus NewStatus { get; set; }
    }

    public class UpdateOrderStatusCommandHandler : IRequestHandler<UpdateOrderStatusCommand, ActionResult>
    {
        private readonly IApplicationContext _context;

        public UpdateOrderStatusCommandHandler(IApplicationContext context)
        {
            _context = context;
        }

        public async Task<ActionResult> Handle(UpdateOrderStatusCommand request, CancellationToken cancellationToken)
        {
            var order = await _context.Orders.FindAsync(request.OrderId, cancellationToken);
            if (order == null)
            {
                return new NotFoundResult();
            }
            var status = request.NewStatus;
            order.Status = request.NewStatus;

            await _context.SaveChangesAsync(cancellationToken);

            return new OkResult();
        }
    }
}
