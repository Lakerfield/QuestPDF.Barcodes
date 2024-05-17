using Barcoder.Qr;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using QuestPDF.Previewer;

namespace QuestPdf.Playground
{
  internal class Program
  {
    static void Main(string[] args)
    {
      //QuestPDF.Settings.License = LicenseType.Community;

      var document = Document.Create(container =>
      {
        container.Page(page =>
        {
          page
            .Content()
            .Column(column =>
            {
              column.Item()
                .AlignCenter()
                .Text("Example barcodes")
                .FontSize(20);

              column.Item()
                .AlignCenter()
                .Border(1)
                .Height(2, Unit.Centimetre)
                .Width(10, Unit.Centimetre)
                .BarcodeTwoToFive("1234567890", true, false);

              column.Item()
                .AlignCenter()
                .Height(1, Unit.Centimetre)
                .Width(10, Unit.Centimetre)
                .BarcodeQr("1234567890");

              column.Item()
                .AlignCenter()
                .Border(1).BorderColor(Colors.Red.Medium)
                .Height(1, Unit.Centimetre)
                .Width(10, Unit.Centimetre)
                .BarcodeQr("1234567890");

              column.Item()
                .AlignCenter()
                .Border(1)
                .Height(4, Unit.Centimetre)
                .Width(10, Unit.Centimetre)
                .BarcodeEan("1234567");

              column.Item()
                .AlignCenter()
                .Border(1)
                .Height(4, Unit.Centimetre)
                .Width(10, Unit.Centimetre)
                .BarcodeEan("1234567");

            });
        });
      });

      // instead of the standard way of generating a PDF file
      //document.GeneratePdf("hello.pdf");

      // use the following invocation
      document.ShowInPreviewer(12500);
    }
  }
}
