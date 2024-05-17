using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuestPDF.Drawing
{
  public sealed class BarcodeRenderOptions
  {
    public bool IncludeEanContentAsText { get; set; } = false;

    public int? CustomMargin { get; set; } = null;
  }
}
