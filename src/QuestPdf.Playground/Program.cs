using QuestPDF.Companion;
using QuestPDF.Drawing;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using QuestPDF.Previewer;
using SixLabors.Fonts;

namespace QuestPdf.Playground
{
  internal class Program
  {
    static void Main(string[] args)
    {
      //TestFontSize();

      QuestPDF.Settings.License = LicenseType.Community;

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

              column.AddCenteredItemOfSize(1, 5).BarcodeCode128("1234567890", true, false);
              column.AddCenteredItemOfSize(1, 10).BarcodeCode128("12345678901234567890", true, false);
              column.AddCenteredItemOfSize(1, 10).BarcodeCode128("12345678901234567890", true, false, new BarcodeRenderOptions(){CustomMargin = 40});
              column.AddCenteredItemOfSize(1, 10).BarcodeCode128("123456789012345678901234567890123456789012345678901234567890", true, false);

              column.AddCenteredItemOfSize(2, 10).BarcodeTwoToFive("12345678901234567890", true, false);

              column.AddCenteredItemOfSize(1, 10).BarcodeQr("1234567890");

              column.AddCenteredItemOfSize(1, 10).BarcodeQr("1234567890");

              column.AddCenteredItemOfSize(2, 5).BarcodeEan("1234567");

              column.AddCenteredItemOfSize(2, 5).BarcodeEan("123456789012");

            });
        });
      });

      // instead of the standard way of generating a PDF file
      //document.GeneratePdf("hello.pdf");

      // use the following invocation
      document.ShowInCompanion();
    }

    private static void TestFontSize()
    {
      string fontPath = "LatoFont/Lato-Regular.ttf";

      // Load the font
      FontCollection fontCollection = new FontCollection();
      FontFamily fontFamily = fontCollection.Add(fontPath);

      string test = "0123456789";

      for (int i = 4; i < 32; i++)
      {
        Font font = fontFamily.CreateFont(i); // Font size x points

        FontRectangle size = TextMeasurer.MeasureSize(test, new TextOptions(font));

        // Output the size
        Console.WriteLine($"Font size {i} is: {size.Width}x{size.Height}");
      }

      //Lato regular
      // Width .6 ppl
      // Height .8 ppl => 10% top 80% text 10% bottom
    }
  }

  public static class HelperExtensions
  {
    public static IContainer AddCenteredItemOfSize(this ColumnDescriptor column, float height, float width)
    {
      return column
        .Item()
        .AlignCenter()
        .Height(height, Unit.Centimetre)
        .Width(width, Unit.Centimetre)
        .Border(1).BorderColor(Colors.Red.Medium);
    }

  }
}
