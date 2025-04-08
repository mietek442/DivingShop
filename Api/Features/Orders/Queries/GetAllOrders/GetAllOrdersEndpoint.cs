using Api.Features.Products.Queries.GetAllProducts;
using Api.Features.Products.Queries.GetProducts;
using Ardalis.ApiEndpoints;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Api.Features.Orders.Queries.GetAllOrders
{

    public class GetAllOrdersEndpoint : EndpointBaseAsync.WithoutRequest.WithResult<ActionResult<List<OrderResult>>>
    {
        private readonly IMediator _mediator;

        public GetAllOrdersEndpoint(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("api/orders")]
        [SwaggerOperation(
            Summary = "Get All Orders",
            Description = "Retrieve all Orders from the database with Orders Items",
            OperationId = "Orders_GetAll",
            Tags = new[] { "Orders" })
        ]


        public override async Task<ActionResult<List<OrderResult>>> HandleAsync(CancellationToken cancellationToken = default)
        {

            return await _mediator.Send(new GetOrdersQuery { });
        }
    }
}
