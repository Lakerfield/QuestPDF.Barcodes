using SvgLib;

namespace QuestPDF.Drawing
{
  public static class SvgLibExtensions
  {
    public static string Save(this SvgDocument document)
    {
      using var stream = new MemoryStream();
      document.Save(stream);
      stream.Position = 0;

      using var reader = new StreamReader(stream);
      return reader.ReadToEnd();
    }
  }
}
