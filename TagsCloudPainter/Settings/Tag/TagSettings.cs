using System.Drawing;
using System.Drawing.Text;

namespace TagsCloudPainter.Settings.Tag;

public class TagSettings : ITagSettings
{
    public int TagFontSize { get; set; }
    public FontFamily TagFont { get; set; } = new InstalledFontCollection().Families.FirstOrDefault();
    public Color TagColor { get; set; }
}