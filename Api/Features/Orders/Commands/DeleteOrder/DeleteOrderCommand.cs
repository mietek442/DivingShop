using Api.Infrastructure.DbContext;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Features.Orders.Commands.DeleteOrder
{
    public class DeleteOrderCommand:IRequest<ActionResult<Guid>>
    {
        public Guid OrderId;
    }
    public class DeleteOrderCommandHandler : IRequestHandler<DeleteOrderCommand, ActionResult<Guid>>
    {
        private readonly IApplicationContext _context;
        public DeleteOrderCommandHandler(IApplicationContext context)
        {
            _context = context;
        }
        public async Task<ActionResult<Guid>> Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
        {
            var order = await _context.Orders.FindAsync(request.OrderId, cancellationToken);
            if (order == null)
            {
                return new NotFoundResult();
            }
            order.isIsDeleted = true;

            await _context.SaveChangesAsync(cancellationToken);
            return new OkObjectResult(request.OrderId);
        }
    }
}
