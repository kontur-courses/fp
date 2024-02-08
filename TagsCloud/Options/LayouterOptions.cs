using System.Drawing;

namespace TagsCloud.Options;

public class LayouterOptions
{
    public string InputFile { get; set; }
    public string OutputFile { get; set; }
    public string FontName { get; set; }
    public Size ImageSize { get; set; }
    public Color BackgroundColor{ get; set; }
    public Point Center { get; set; }
}