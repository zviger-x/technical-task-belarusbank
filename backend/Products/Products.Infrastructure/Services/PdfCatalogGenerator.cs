using Products.Application.Contracts;
using Products.Application.Services.Interfaces;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace Products.Infrastructure.Services
{
    public class PdfCatalogGenerator : IPdfCatalogGenerator
    {
        public byte[] Generate(IEnumerable<ProductCatalogItemDto> products)
        {
            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Margin(30);

                    page.Header()
                        .PaddingBottom(10)
                        .Text("Каталог продуктов")
                        .FontSize(20)
                        .Bold();

                    page.Content().Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            columns.RelativeColumn(2);
                            columns.RelativeColumn(2);
                            columns.ConstantColumn(70);
                            columns.RelativeColumn(3);
                            columns.RelativeColumn(2);
                            columns.RelativeColumn(2);
                        });

                        table.Header(header =>
                        {
                            header.Cell().Element(Block).Text("Название").Bold();
                            header.Cell().Element(Block).Text("Категория").Bold();
                            header.Cell().Element(Block).Text("Цена").Bold();
                            header.Cell().Element(Block).Text("Описание").Bold();
                            header.Cell().Element(Block).Text("Общее примечание").Bold();
                            header.Cell().Element(Block).Text("Особое примечание").Bold();
                        });

                        foreach (var product in products)
                        {
                            table.Cell().Element(RowBlock).Text(product.Name);
                            table.Cell().Element(RowBlock).Text(product.Category);
                            table.Cell().Element(RowBlock).Text(product.Price.ToString("F2"));
                            table.Cell().Element(RowBlock).Text(product.Description);
                            table.Cell().Element(RowBlock).Text(product.GeneralNote);
                            table.Cell().Element(RowBlock).Text(product.SpecialNote);
                        }
                    });
                });
            });

            return document.GeneratePdf();
        }

        private static IContainer Block(IContainer container)
        {
            return container
                .BorderBottom(1)
                .BorderColor(Colors.Grey.Lighten2)
                .Padding(5);
        }

        private static IContainer RowBlock(IContainer container)
        {
            return container
                .ShowEntire()
                .MinHeight(20)
                .Padding(5)
                .AlignMiddle();
        }
    }
}
