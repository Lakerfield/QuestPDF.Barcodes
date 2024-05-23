using QuestPDF.Infrastructure;
using Barcoder;
using QuestPDF.Drawing;
using SkiaSharp;

namespace QuestPDF.Fluent;

public static class BarcodeExtensions
{
  public static void Barcode(this IContainer container, IBarcode barcode, BarcodeRenderOptions? options = default)
  {
    options ??= new BarcodeRenderOptions();
    container.Canvas((SKCanvas canvas, Size size) =>
    {
      var renderer = new SvgBarcodeRenderer(options);

      using var ms = new MemoryStream();
      renderer.Render(barcode, size, ms);
      ms.Position = 0;

      var skSvg = options.CustomRenderResolution.HasValue ? new SKSvg(options.CustomRenderResolution.Value) : new SKSvg();
      skSvg.Load(ms);

      float scaleX = size.Width / skSvg.Picture.CullRect.Width;
      float scaleY = size.Height / skSvg.Picture.CullRect.Height;
      float scale = (scaleX < scaleY) ? scaleX : scaleY;
      canvas.Scale(scale, scale);

      canvas.DrawPicture(skSvg.Picture);
    });
  }

  public static void BarcodeAztec(this IContainer container, string content, int minimumEccPercentage = 23, int userSpecifiedLayers = 0, BarcodeRenderOptions? options = null)
  {
    container.Barcode(Barcoder.Aztec.AztecEncoder.Encode(content, minimumEccPercentage, userSpecifiedLayers), options);
  }

  public static void BarcodeCodebar(this IContainer container, string content, BarcodeRenderOptions? options = null)
  {
    container.Barcode(Barcoder.Codebar.CodabarEncoder.Encode(content), options);
  }

  public static void BarcodeCode128(this IContainer container, string content, bool includeChecksum = true, bool gs1ModeEnabled = false, BarcodeRenderOptions? options = null)
  {
    container.Barcode(Barcoder.Code128.Code128Encoder.Encode(content, includeChecksum, gs1ModeEnabled), options);
  }

  public static void BarcodeCode39(this IContainer container, string content, bool includeChecksum, bool fullAsciiMode, BarcodeRenderOptions? options = null)
  {
    container.Barcode(Barcoder.Code39.Code39Encoder.Encode(content, includeChecksum, fullAsciiMode), options);
  }

  public static void BarcodeCode93(this IContainer container, string content, bool includeChecksum, bool fullAsciiMode, BarcodeRenderOptions? options = null)
  {
    container.Barcode(Barcoder.Code93.Code93Encoder.Encode(content, includeChecksum, fullAsciiMode), options);
  }

  public static void BarcodeDataMatrix(this IContainer container, string content, int? fixedNumberOfRows = null, int? fixedNumberOfColumns = null, bool gs1ModeEnabled = false, BarcodeRenderOptions? options = null)
  {
    container.Barcode(Barcoder.DataMatrix.DataMatrixEncoder.Encode(content, fixedNumberOfRows, fixedNumberOfColumns, gs1ModeEnabled), options);
  }

  public static void BarcodeEan(this IContainer container, string content, BarcodeRenderOptions? options = null)
  {
    container.Barcode(Barcoder.Ean.EanEncoder.Encode(content), options);
  }

  public static void BarcodeKix(this IContainer container, string content, BarcodeRenderOptions? options = null)
  {
    container.Barcode(Barcoder.Kix.KixEncoder.Encode(content), options);
  }

  public static void BarcodePdf417(this IContainer container, string content, byte securityLevel, BarcodeRenderOptions? options = null)
  {
    container.Barcode(Barcoder.Pdf417.Pdf417Encoder.Encode(content, securityLevel), options);
  }

  public static void BarcodeQr(this IContainer container, string content, Barcoder.Qr.ErrorCorrectionLevel errorCorrectionLevel = Barcoder.Qr.ErrorCorrectionLevel.M, Barcoder.Qr.Encoding encoding = Barcoder.Qr.Encoding.Auto, BarcodeRenderOptions? options = null)
  {
    container.Barcode(Barcoder.Qr.QrEncoder.Encode(content, errorCorrectionLevel, encoding), options);
  }

  public static void BarcodeRoyalMail(this IContainer container, string content, BarcodeRenderOptions? options = null)
  {
    container.Barcode(Barcoder.RoyalMail.RoyalMailFourStateCodeEncoder.Encode(content), options);
  }

  public static void BarcodeTwoToFive(this IContainer container, string content, bool interleaved, bool includeChecksum, BarcodeRenderOptions? options = null)
  {
    container.Barcode(Barcoder.TwoToFive.TwoToFiveEncoder.Encode(content, interleaved, includeChecksum), options);
  }

  public static void BarcodeUpcA(this IContainer container, string content, BarcodeRenderOptions? options = null)
  {
    container.Barcode(Barcoder.UpcA.UpcAEncoder.Encode(content), options);
  }

  public static void BarcodeUpcE(this IContainer container, string content, Barcoder.UpcE.UpcENumberSystem numberSystem, BarcodeRenderOptions? options = null)
  {
    container.Barcode(Barcoder.UpcE.UpcEEncoder.Encode(content, numberSystem), options);
  }

}
