using Api.Domain.Models;
using Api.Infrastructure.DbContext;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Features.ProductParams.Commands.UpdateProductParam
{
    public class UpdateProductParamCommand : IRequest<ActionResult<ProductParam>>
    {
        public Guid ProductParamId { get; set; }
        public UpdateProductParamModelRequest ProductParamRequest { get; set; }
    }

    public class UpdateProductParamCommandHandler : IRequestHandler<UpdateProductParamCommand, ActionResult<ProductParam>>
    {
        private readonly IApplicationContext _context;

        public UpdateProductParamCommandHandler(IApplicationContext context)
        {
            _context = context;
        }

        public async Task<ActionResult<ProductParam>> Handle(UpdateProductParamCommand request, CancellationToken cancellationToken)
        {
           
            var productParam = await _context.ProductParams.FindAsync(request.ProductParamId, cancellationToken);
            if (productParam == null)
            {
                return new NotFoundResult();
            }
            UpdateProductParamMapper.MapToProductParam(productParam, request.ProductParamRequest);

            _context.ProductParams.Update(productParam);
            await _context.SaveChangesAsync(cancellationToken);

            return new OkObjectResult(productParam);
        }
    }
}
