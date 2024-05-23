using QuestPDF.Helpers;

namespace QuestPDF.Drawing
{
  public sealed class BarcodeRenderOptions
  {
    public bool IncludeContentAsText { get; set; } = true;

    public string Font { get; set; } = Fonts.Lato;

    public int? CustomMargin { get; set; } = null;

    public float StrokeWidthCorrection1D { get; set; } = 0.2f;

    public int? CustomRenderResolution { get; set; } = null;
  }
}
