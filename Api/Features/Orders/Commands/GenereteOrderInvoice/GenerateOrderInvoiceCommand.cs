using Api.Infrastructure.DbContext;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using static Api.Features.Orders.Queries.GetOrderById.GetOrderByIdQueryHandler;

namespace Api.Features.Orders.Commands.GenerateOrderInvoice
{
    public class GenerateOrderInvoiceCommand : IRequest<IResult>
    {
        public Guid OrderId { get; set; }
    }

    public class GenerateOrderInvoiceResult
    {
        public byte[] PdfBytes { get; set; } = Array.Empty<byte>();
        public string FileName { get; set; } = string.Empty;
    }

    public class GenerateOrderInvoiceCommandHandler : IRequestHandler<GenerateOrderInvoiceCommand, IResult>
    {
        private readonly IApplicationContext _context;

        public GenerateOrderInvoiceCommandHandler(IApplicationContext context)
        {
            _context = context;
        }

        public async Task<IResult> Handle(GenerateOrderInvoiceCommand request, CancellationToken cancellationToken)
        {
            var order = await _context.Orders
                    .Where(o => o.Id == request.OrderId)
                    .Select(o => new OrderByIDto
                    {
                        Id = o.Id,
                        CreatedAt = o.CreatedAt,
                        Status = o.Status,
                        UserName = o.UserName,
                        UserLastName = o.UserLastName,
                        Address = o.Address,
                        PhoneNumber = o.PhoneNumber,
                        Email = o.Email,
                        OrderNumber = o.OrderNumber,
                        Price = o.Price,
                        ShipPrice = o.ShipPrice,
                        DeliveryMethod = o.DeliveryMethod,
                        TotalPrice = o.TotalPrice,
                        TotalPriceIncludeTax = o.TotalPriceIncludeTax,
                        Items = o.OrderItems.Select(oi => new OrderByIDItemDto
                        {
                            Id = oi.Id,
                            Quantity = oi.Quantity,
                            ProductTitle = oi.Product.Title,
                            ProductShortDesc = oi.Product.ShortDesc,
                            TotalProductsPrice = oi.TotalProductsPrice,
                            ImageUrl = oi.Product.ImageUrls.Count > 0 ? oi.Product.ImageUrls[0] : null,
                        }).ToList()
                    })
                    .FirstOrDefaultAsync(cancellationToken);
            if (order == null)
            {
                return Results.NotFound();
            }
            QuestPDF.Settings.License = LicenseType.Community;
            // Tworzymy dokument QuestPDF
            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(40);

                    // HEADER
                    page.Header().Column(header =>
                    {
                        header.Item().Text("Faktura VAT").FontSize(24).Bold().FontColor(Colors.Black);
                        header.Item().PaddingTop(5).LineHorizontal(1).LineColor(Colors.Grey.Darken2);
                    });

                    // SELLER & BUYER INFO
                    page.Content().Column(col =>
                    {
                        col.Item().Row(row =>
                        {
                            row.RelativeColumn().Column(seller =>
                            {
                                seller.Item().Text("Sprzedawca:").FontSize(12).SemiBold();
                                seller.Item().Text("NurkowanieWSzambie.pl Sp. z o.o.");
                                seller.Item().Text("ul.  Obrońców Ostatniego Łańcucha Branży Spożywczej");
                                seller.Item().Text("32-000 Warszawa");
                                seller.Item().Text("NIP: 77412312898632");
                            });

                            row.RelativeColumn().Column(buyer =>
                            {
                                buyer.Item().Text("Nabywca:").FontSize(12).SemiBold();
                                buyer.Item().Text($"Pan/Pani: {order.UserName} {order.UserLastName}");
                                buyer.Item().Text(order.Address);
                                buyer.Item().Text($"Numer zamówienia: {order.OrderNumber}");
                                buyer.Item().Text($"Data: {order.CreatedAt:yyyy-MM-dd}");
                            });
                        });

                        col.Item().PaddingVertical(20);

                        // TABLE HEADER
                        col.Item().Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.ConstantColumn(40);  // LP
                                columns.ConstantColumn(80);  // nazwa
                                columns.ConstantColumn(190); // opis
                                columns.ConstantColumn(80);  // ilość
                                columns.ConstantColumn(100); // Cena brutto
                            });

                            table.Header(header =>
                            {
                                header.Cell().Text("LP").Bold();
                                header.Cell().Text("Nazwa produktu").Bold();
                                header.Cell().Text("Opis").Bold();
                                header.Cell().Text("Ilość").Bold();
                                header.Cell().Text("Cena Netto").Bold();
                            });

                            int lp = 1;
                            foreach (var item in order.Items)
                            {
                                table.Cell().Text(lp++);

                                table.Cell().Text(item.ProductTitle);

                                var desc = item.ProductShortDesc ?? "";
                                if (desc.Length > 30) // przytnij opis aby zmieścił się w 190px
                                    desc = desc.Substring(0, 30) + "...";

                                table.Cell().Text(desc);

                                table.Cell().Text($"{item.Quantity}");
                                table.Cell().Text($"{item.TotalProductsPrice} zł");
                            }
                        });

                        col.Item().PaddingVertical(20);
                        // SUMMARY
                        col.Item().AlignRight().Column(summary =>
                        {
                            summary.Item().Text($"Cena produktów (Netto): {order.Price} zł");
                            summary.Item().Text($"Koszt dostawy:: {order.ShipPrice} zł");
                            summary.Item().Text($"Cena całkowita (netto + dostawa):: {order.TotalPrice} zł");
                          
                            summary.Item().Text($"Cena Brutto: {order.TotalPriceIncludeTax}  zł").Bold();
                        });
                    });

                    // FOOTER
                    page.Footer().AlignCenter().Text(x =>
                    {
                        x.Span("Wygenerowano automatycznie systemem ").FontSize(8);
                        x.Span(TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow,
                            TimeZoneInfo.FindSystemTimeZoneById("Central European Standard Time"))
                            .ToString("yyyy-MM-dd HH:mm"))
                            .FontSize(8).Italic();
                    });
                });
            });

            byte[] pdfBytes = document.GeneratePdf();

            var result = new GenerateOrderInvoiceResult
            {
                PdfBytes = pdfBytes,
                FileName = $"invoice-{order.OrderNumber}.pdf"
            };

            return Results.File(result.PdfBytes, "application/pdf", result.FileName);
        }
    }
}

/*
 using Api.Infrastructure.DbContext;
using HandlebarsDotNet;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Playwright;
using PdfSharp.Pdf;
using PdfSharp;
using PuppeteerSharp;
using PuppeteerSharp.Media;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TheArtOfDev.HtmlRenderer.PdfSharp;

namespace Api.Features.Orders.Commands.GenereteOrderInvoice
{
    public class GenerateOrderInvoiceCommand : IRequest<IResult>
    {
        public Guid OrderId { get; set; }
    }

    public class GenerateOrderInvoiceCommandHandler : IRequestHandler<GenerateOrderInvoiceCommand, IResult>
    {
        private readonly IApplicationContext _context;

        public GenerateOrderInvoiceCommandHandler(IApplicationContext context)
        {
            _context = context;
        }

        public async Task<IResult> Handle(GenerateOrderInvoiceCommand request, CancellationToken cancellationToken)
        {
            var order = await _context.Orders.FindAsync(request.OrderId, cancellationToken);
            if (order == null)
            {
                return Results.NotFound();
            }
            string templateContent = @"
<html>
<head>
    <meta charset='utf-8'>
    <title>FAKTURA VAT</title>
</head>
<body>
    <h2>From:</h2>
    <p>{{UserName}}</p>
    <p>{{Address}}</p>
</body>
</html>";

            var template = Handlebars.Compile(templateContent);

           
            var data = new
            {
                UserName = order.UserName,
                Address = order.Address
            };

         
            var html = template(data);

           
            await File.WriteAllTextAsync(
                "Faktura.html",
                html,
                new UTF8Encoding(encoderShouldEmitUTF8Identifier: false)
            );
            using var ms = new MemoryStream();
            using var pdf = new PdfDocument();

            // Renderowanie HTML → PDF
            object value = PdfGenerator.AddPdfPages(pdf, html, PageSize.A4);

            // Zapis do MemoryStream
            pdf.Save(ms, false);

            return new GenereteOrderInvoiceResult
            {
                PdfBytes = ms.ToArray(),
                FileName = $"invoice-{orderNumber}.pdf"
            };
        }
        var test = new GenereteOrderInvoiceResult
            {
                InvoiceID = order.Address
            };
            return Results.File(pdfData, "application/pdf", $"invoice-{order.OrderNumber}.pdf");

        }
    }
}




*/