using Api.Domain.Models;
using Api.Features.Common.Services.Storage;
using Api.Infrastructure.DbContext;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Features.Products.Commands.DeleteProduct
{
    public class DeleteProductCommand : IRequest<ActionResult>
    {
        public Guid ProductId { get; set; }
    }

    public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand, ActionResult>
    {
        private readonly IApplicationContext _context;

        public DeleteProductCommandHandler(IApplicationContext context)
        {
            _context = context;
        }

        public async Task<ActionResult> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            var product = await _context.Products.FindAsync(new object[] { request.ProductId }, cancellationToken);
            if (product == null)
            {
                return new NotFoundResult();
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync(cancellationToken);

            return new NoContentResult();
        }
    }
}
