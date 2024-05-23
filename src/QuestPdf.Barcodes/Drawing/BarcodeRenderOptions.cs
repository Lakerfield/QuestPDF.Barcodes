using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuestPDF.Drawing
{
  public sealed class BarcodeRenderOptions
  {
    public bool IncludeContentAsText { get; set; } = true;

    public int? CustomMargin { get; set; } = null;

    public float StrokeWidthCorrection1D { get; set; } = 0.2f;
  }
}
