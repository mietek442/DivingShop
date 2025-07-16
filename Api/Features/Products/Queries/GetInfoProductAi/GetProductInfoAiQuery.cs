using Api.Domain.Models;
using Api.Features.Common.Services.UrlHelper;
using Api.Features.Products.Commands.Common.Models;
using Api.Infrastructure.DbContext;
using Deepseek.AspClient.Client;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Features.Products.Queries.GetInfoProductAi
{
    public class GetProductInfoAiQuery : IRequest<ActionResult<ProductInfoAiResult>>
    {
        public Guid Id { get; set; }
        public string Question { get; set; }
    }

    public class GetProductInfoAiQueryHandler : IRequestHandler<GetProductInfoAiQuery, ActionResult<ProductInfoAiResult>>
    {
        private readonly IApplicationContext _context;
        private readonly IUrlHelpers _urlHelpers;
        private readonly DeepseekClient _client;
        public GetProductInfoAiQueryHandler(IApplicationContext context,DeepseekClient client)
        {
            _context = context;
           
            _client = client ?? throw new ArgumentNullException(nameof(client));
        }

        public async Task<ActionResult<ProductInfoAiResult>> Handle(GetProductInfoAiQuery request, CancellationToken cancellationToken)
        {
            var product = await _context.Products.FindAsync(request.Id);
            if (product == null)
            {
                return new NotFoundResult();
            }

            var prompt = $"Mając Informacje o produkcie:  firma: {product.Manufacture}, Tytuł: {product.Title}, Opis: {product.Description}, Cena bazowa: {product.BasePrice}, Dostępność: {product.Available} odpowiedź na pytanie: {request.Question} napisz na maks 100 słów, korzystaj tylko z tych danych co wysłałem, jesli nie znasz odpowiedzi zwróc: Skontaktuj sie z pomocą techniczną w linku poniżej, nie pisz bezsensownych wiadomosci ";
            var response = await _client.GenerateResponseAsync(prompt);
            var content = response.Choices.FirstOrDefault()?.Message.Content;

            if (string.IsNullOrWhiteSpace(content))
            {
                return new NotFoundObjectResult("No response content received from Deepseek.");
            }

            ProductInfoAiResult result = new ProductInfoAiResult { Response = content };
            return result;
        }
    }
}
