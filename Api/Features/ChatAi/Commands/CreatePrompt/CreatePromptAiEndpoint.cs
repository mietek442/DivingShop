namespace Api.Features.ChatAi.Commands.CreatePrompt
{
    using Ardalis.ApiEndpoints;
    using Deepseek.AspClient.Client;
    using MediatR;
    using Microsoft.AspNetCore.Mvc;
    using Swashbuckle.AspNetCore.Annotations;
    using System.Threading;
    using System.Threading.Tasks;

    public class CreatePromptAiEndpoint : EndpointBaseAsync
        .WithRequest<CreatePromptRequest>
        .WithActionResult<CreatePromptResult>
    {
        private readonly IMediator _mediator;
        private readonly DeepseekClient _client;
        public CreatePromptAiEndpoint(IMediator mediator, DeepseekClient client)
        {
            _mediator = mediator;
            _client = client ?? throw new ArgumentNullException(nameof(client));
        }

        [HttpPost("api/chat/prompt")]
        [SwaggerOperation(
            Summary = "Creates a new AI prompt",
            Description = "Creates a new AI prompt based on the provided string input",
            OperationId = "ChatAi_CreatePrompt",
            Tags = new[] { "ChatAI" })]
        public override async Task<ActionResult<CreatePromptResult>> HandleAsync(
            CreatePromptRequest request,
            CancellationToken cancellationToken = default)
        {

            var response = await _client.GenerateResponseAsync("esteś chatem który ma informacje na temat:        ssrzedam BMW e36 z silnikiem m43b19 - z rocznika 2000\r\n\r\n-lusterka fake m3\r\n- sprężyny -35/40mm eibach\r\n-tuleje acentryczne m3 poliuretanowe\r\n- wnętrze grzane sporty\r\n- nowa chłodnica wody Nissens\r\n- światła przednie zrobione na Yellow Hella\r\n- szeroka listwa\r\n- usunięta wycieraczka tylna (mam ją jakby co)\r\n- wiele innych , lecz nie wszytko pamiętam\r\n- 2x kluczyk\r\n- zamek centralny\r\nMówiąca w skrócie wszytko co jest na wyposażeniu auta jest sprawne.\r\n- 2 komplety kół letnie i zimowe\r\n\r\npodłoga w fajnym stanie, Mocowania wózka\r\nstabilizatora idealne. Z zewnątrz wygląda\r\nbardzo dobrze , nałożona ceramika.\r\nWnętrze mega zadbane,czyste i niezniszczone.\r\nKlimatyzacja jest w pelni sprawna, grzane fotele też, fotochromatyczne lusterko również!\r\n\r\nAuto w 100% sprawne gotowe do jazdy , auta używałem na codzień. Jest to zadbane e36 compact.\r\nDla konkretnych dodam osobno dyfer szpera 25% oraz rozpórki przód i tył dedykowane do e36 , praktycznie niemożliwe dokupienia.\r\nŚwieżo wymieniony olej.\r\nPrzegląd techniczny : 04.09.2025\r\nOc: 7.11.2025\r\nSprzedaje tylko i wyłącznie ponieważ - muszę kupić większe auto.\r\nPosiadam więcej zdjęć mogę podesłać.\r\nŚmiało można dzwonić po więcej informacji 69*******43.\r\nCENA DO NEGOCJACJI - napewno sie dogadamy.     Bmw e36 compact m43b19\r\n12 500 zł\r\n  oraz korzytajac z informacji z internetu odpowiedz mi prosze na pytanie:    " + request.Prompt+ "maksymalnie 100 słów, używaj nie zbyt miłego jezyka, prosto, klinenta nawiij jełopem, znaczy zwrcaj się jełpopie  i tak dalej ");


            var content = response.Choices.FirstOrDefault()?.Message.Content; 

            if (string.IsNullOrWhiteSpace(content)) 
            {
                return new NotFoundObjectResult("No response content received from Deepseek.");
            }

            
            var result = new CreatePromptResult() { Response = content };
            return Ok(result);
        }
    }

    public class CreatePromptRequest
    {
        public string Prompt { get; set; }
    }

    public class CreatePromptResult
    {
        public string Response { get; set; }
    }

    public class CreatePromptCommand : IRequest<CreatePromptResult>
    {
        public string Prompt { get; set; }
    }
}
