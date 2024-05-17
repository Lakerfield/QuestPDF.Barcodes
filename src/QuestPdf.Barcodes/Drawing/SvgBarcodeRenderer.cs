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

    public SvgBarcodeRenderer(BarcodeRenderOptions options = null)
    {
      _options = options ?? new BarcodeRenderOptions();
    }

    private bool IncludeEanContent(IBarcode barcode)
        => _options.IncludeEanContentAsText && (barcode.Metadata.CodeKind == BarcodeType.EAN13 || barcode.Metadata.CodeKind == BarcodeType.EAN8);

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
      var document = SvgDocument.Create();
      int height = IncludeEanContent(barcode) ? 55 : 50;
      int margin = _options.CustomMargin ?? barcode.Margin;

      document.ViewBox = new SvgViewBox
      {
        Left = 0,
        Top = 0,
        Width = barcode.Bounds.X + 2 * margin,
        Height = height
      };
      document.Fill = "#FFFFFF";
      document.Stroke = "#000000";
      document.StrokeWidth = 1;
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
        int lineHeight = height;
        if (IncludeEanContent(barcode))
        {
          if (barcode.Metadata.CodeKind == BarcodeType.EAN13)
          {
            if (!Ean13LongerBars.Contains(x))
            {
              lineHeight = 48;
            }
          }
          else
          {
            if (!Ean8LongerBars.Contains(x))
            {
              lineHeight = 48;
            }
          }
        }

        if (prevBar)
        {
          line = document.AddLine();
          line.StrokeWidth = 1.5;
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

      if (IncludeEanContent(barcode))
      {
        if (barcode.Metadata.CodeKind == BarcodeType.EAN13)
        {
          AddText(document, 4, 54.5D, barcode.Content.Substring(0, 1));
          AddText(document, 21, 54.5D, barcode.Content.Substring(1, 6));
          AddText(document, 67, 54.5D, barcode.Content.Substring(7));
        }
        else
        {
          AddText(document, 18, 54.5D, barcode.Content.Substring(0, 4));
          AddText(document, 50, 54.5D, barcode.Content.Substring(4));
        }
      }

      return document.OuterXml;
    }

    private void AddText(SvgDocument doc, double x, double y, string t)
    {
      SvgText text = doc.AddText();
      text.FontFamily = Fonts.Lato;
      text.Text = t;
      text.X = x;
      text.Y = y;
      text.StrokeWidth = 0;
      text.Fill = "#000000";
      text.FontSize = 8D;
      text.TextAnchor = SvgTextAnchor.Middle;
      var l = text.TextLength;
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

      return document.OuterXml;
    }
  }
}
