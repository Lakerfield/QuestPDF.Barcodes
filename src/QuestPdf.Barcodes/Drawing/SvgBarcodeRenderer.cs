using Barcoder.Renderers;
using Barcoder;
using SvgLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace QuestPDF.Drawing
{
  public sealed class SvgBarcodeRenderer
  {
    private static readonly int[] Ean8LongerBars = new[] { 0, 2, 32, 34, 64, 66 };
    private static readonly int[] Ean13LongerBars = new[] { 0, 2, 46, 48, 92, 94 };

    private readonly BarcodeRenderOptions _options;

    public SvgBarcodeRenderer(BarcodeRenderOptions? options = null)
    {
      _options = options ?? new BarcodeRenderOptions();
    }

    public string Render(IBarcode barcode, Size size)
    {
      barcode = barcode ?? throw new ArgumentNullException(nameof(barcode));
      if (barcode.Bounds.Y == 1)
        return Render1D(barcode, size);
      else if (barcode.Bounds.Y > 1)
        return Render2D(barcode, size);
      else
        throw new NotSupportedException($"Y value of {barcode.Bounds.Y} is invalid");
    }

    private string Render1D(IBarcode barcode, Size size)
    {
      int margin = _options.CustomMargin ?? barcode.Margin;

      double width = barcode.Bounds.X + 2 * margin;
      double height = size.Height / size.Width * width;
      double heightShort = height*4/5;
      double heightText = height - heightShort;
      double fontsize = heightText / 0.9d;
      double textWidth = (barcode.Content.Length + 2) * 0.6d * fontsize;
      int textStart = (int)Math.Floor(((barcode.Bounds.X - textWidth) / 2) );
      int textEnd = (int)Math.Ceiling(((barcode.Bounds.X + textWidth) / 2) );

      var document = SvgDocument.Create();
      document.ViewBox = new SvgViewBox
      {
        Left = 0,
        Top = 0,
        Width = barcode.Bounds.X + 2 * margin,
        Height = height
      };
      document.Fill = "#FFFFFF";
      document.Stroke = "#000000";
      document.StrokeWidth = 1f - _options.StrokeWidthCorrection1D;
      document.StrokeLineCap = SvgStrokeLineCap.Butt;

      var prevBar = false;
      for (var x = 0; x < barcode.Bounds.X; x++)
      {
        if (!barcode.At(x, 0))
        {
          prevBar = false;
          continue;
        }

        SvgLine line;
        double lineHeight = height;
        if (_options.IncludeContentAsText)
        {
          switch (barcode.Metadata.CodeKind)
          {
            case BarcodeType.EAN13:
              if (!Ean13LongerBars.Contains(x))
                lineHeight = heightShort;
              break;

            case BarcodeType.EAN8:
              if (!Ean8LongerBars.Contains(x))
                lineHeight = heightShort;
              break;

            default:
              if (x >= textStart && x <= textEnd)
                lineHeight = heightShort;
              break;
          }
        }

        if (prevBar)
        {
          line = document.AddLine();
          line.StrokeWidth = 1.5f - _options.StrokeWidthCorrection1D;
          line.X1 = line.X2 = x + margin - 0.25;
          line.Y1 = 0;
          line.Y2 = lineHeight;
        }
        else
        {
          line = document.AddLine();
          line.X1 = line.X2 = x + margin;
          line.Y1 = 0;
          line.Y2 = lineHeight;
        }

        prevBar = true;
      }

      if (_options.IncludeContentAsText)
      {
        var textBase = height - (heightText * 0.1d);
        switch (barcode.Metadata.CodeKind)
        {
          case BarcodeType.EAN13:
            AddText(document, 6 + margin, textBase, barcode.Content.Substring(0, 1), fontsize);
            AddText(document, 26 + margin, textBase, barcode.Content.Substring(1, 6), fontsize);
            AddText(document, 70 + margin, textBase, barcode.Content.Substring(7), fontsize);

            break;

          case BarcodeType.EAN8:
            AddText(document, 17 + margin, textBase, barcode.Content.Substring(0, 4), fontsize);
            AddText(document, 49 + margin, textBase, barcode.Content.Substring(4), fontsize);
            break;

          default:
            AddText(document, barcode.Bounds.X / 2d + margin, textBase, barcode.Content, fontsize);
            break;
        }

        static void AddText(SvgDocument doc, double x, double y, string t, double fontSize)
        {
          SvgText text = doc.AddText();
          text.FontFamily = Fonts.Lato;
          text.Text = t;
          text.X = x;
          text.Y = y;
          text.StrokeWidth = 0;
          text.Fill = "#000000";
          text.FontSize = fontSize;
          text.TextAnchor = SvgTextAnchor.Middle;
        }
      }

      return document.Save();
    }

    private string Render2D(IBarcode barcode, Size size)
    {
      int margin = _options.CustomMargin ?? barcode.Margin;

      var document = SvgDocument.Create();
      document.ViewBox = new SvgViewBox
      {
        Left = 0,
        Top = 0,
        Width = barcode.Bounds.X + 2 * margin,
        Height = barcode.Bounds.Y + 2 * margin
      };
      document.Fill = "#FFFFFF";
      document.Stroke = "#000000";
      document.StrokeWidth = .05;
      document.StrokeLineCap = SvgStrokeLineCap.Butt;

      SvgGroup group = document.AddGroup();
      group.Fill = "#000000";
      for (int y = 0; y < barcode.Bounds.Y; y++)
      {
        for (int x = 0; x < barcode.Bounds.X; x++)
        {
          if (barcode.At(x, y))
          {
            SvgRect rect = group.AddRect();
            rect.X = x + margin;
            rect.Y = y + margin;
            rect.Width = 1;
            rect.Height = 1;
          }
        }
      }

      return document.Save();
    }
  }
}
