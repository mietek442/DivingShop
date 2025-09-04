using Api.Infrastructure.DbContext;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ZXing;
using ZXing.QrCode;
using ZXing.Rendering;
using ZXing.OneD;
using static Api.Features.Orders.Queries.GetOrderById.GetOrderByIdQueryHandler;
using Api.Domain.Models;

namespace Api.Features.Orders.Commands.GenerateOrderLabel
{
    public class GenerateOrderLabelCommand : IRequest<IResult>
    {
        public Guid OrderId { get; set; }
    }

    public class GenerateOrderLabelResult
    {
        public byte[] PdfBytes { get; set; } = Array.Empty<byte>();
        public string FileName { get; set; } = string.Empty;
    }

    public class GenerateOrderLabelCommandHandler : IRequestHandler<GenerateOrderLabelCommand, IResult>
    {
        private readonly IApplicationContext _context;

        public GenerateOrderLabelCommandHandler(IApplicationContext context)
        {
            _context = context;
        }

        public async Task<IResult> Handle(GenerateOrderLabelCommand request, CancellationToken cancellationToken)
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
                return Results.NotFound();

            QuestPDF.Settings.License = LicenseType.Community;

            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A6);
                    page.Margin(20);

                    page.Header().Column(header =>
                    {
                        header.Item().Text("Etykieta wysyłkowa").FontSize(16).Bold().FontColor(Colors.Black);
                        header.Item().PaddingTop(5).LineHorizontal(1).LineColor(Colors.Grey.Darken2);
                    });

                    page.Content().Row(row =>
                    {
                        row.RelativeColumn(2).Column(col =>
                        {
                            col.Item().Text($"Nr zamówienia: {order.OrderNumber}").FontSize(12).Bold();
                            col.Item().Text($"Data: {order.CreatedAt:yyyy-MM-dd}");
                            col.Item().PaddingVertical(10);
                            col.Item().Text("Adres dostawy:").FontSize(12).SemiBold();
                            col.Item().Text($"{order.UserName} {order.UserLastName}");
                            col.Item().Text(order.Address);
                            col.Item().Text($"Tel: {order.PhoneNumber}");
                            col.Item().Text($"Email: {order.Email}");
                            col.Item().PaddingVertical(10);
                            col.Item().Text($"Metoda dostawy: {order.DeliveryMethod}").FontSize(11);
                            col.Item().Text($"Koszt dostawy: {order.ShipPrice} zł");
                            col.Item().PaddingTop(10).Width(100).Height(100).Background(Colors.White).Svg(size =>
                            {
                                var writer = new QRCodeWriter();
                                var qrCode = writer.encode(order.OrderNumber+order.Address, BarcodeFormat.QR_CODE, (int)size.Width, (int)size.Height);
                                var renderer = new SvgRenderer { FontName = "Lato" };
                                return renderer.Render(qrCode, BarcodeFormat.QR_CODE, null).Content;
                            });
                        });

                        row.RelativeColumn(3).AlignCenter().AlignMiddle().Element(container =>
                        {
                            container.Width(154).Height(150).Svg(size =>
                            {
                                var writer = new Code128Writer();
                                var code = writer.encode("nr:"+order.OrderNumber, BarcodeFormat.CODE_128, 350, 120);
                                var renderer = new SvgRenderer { FontName = "Lato", FontSize = 18 };
                                var svgContent = renderer.Render(code, BarcodeFormat.CODE_128, order.OrderNumber).Content;
                                var rotatedSvg = svgContent.Replace("<svg ", $"<svg width=\"350\" transform=\"rotate(90 {(size.Width + 15 / 2)} {50})\" ");
                                return rotatedSvg;
                            });
                        });
                    });

                    page.Footer().AlignCenter().Text(x =>
                    {
                        x.Span("Etykieta wygenerowana automatycznie ").FontSize(8);
                        x.Span(TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow,
                                TimeZoneInfo.FindSystemTimeZoneById("Central European Standard Time"))
                            .ToString("yyyy-MM-dd HH:mm"))
                            .FontSize(8).Italic();
                    });
                });
            });

            var result = new GenerateOrderLabelResult
            {
                PdfBytes = document.GeneratePdf(),
                FileName = $"label-{order.OrderNumber}.pdf"
            };

            return Results.File(result.PdfBytes, "application/pdf", result.FileName);
        }
    }
}
