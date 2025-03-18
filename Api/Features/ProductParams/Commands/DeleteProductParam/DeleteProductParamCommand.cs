using Api.Domain.Models;
using Api.Infrastructure.DbContext;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Features.ProductParams.Commands.DeleteProductParam
{
    public class DeleteProductParamCommand : IRequest<ActionResult>
    {
        public Guid ProductParamId { get; set; }
    }

    public class DeleteProductParamCommandHandler : IRequestHandler<DeleteProductParamCommand, ActionResult>
    {
        private readonly IApplicationContext _context;

        public DeleteProductParamCommandHandler(IApplicationContext context)
        {
            _context = context;
        }

        public async Task<ActionResult> Handle(DeleteProductParamCommand request, CancellationToken cancellationToken)
        {
            var productParam = await _context.ProductParams.FindAsync(request.ProductParamId, cancellationToken);
            if (productParam == null)
            {
                return new NotFoundResult();
            }

            _context.ProductParams.Remove(productParam);
            await _context.SaveChangesAsync(cancellationToken);

            return new NoContentResult();
        }
    }
}
