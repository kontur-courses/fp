using System.Drawing;

namespace TagCloud.settings
{
    public interface ITagSettings
    {
        FontFamily GetFontFamily();
        float GetStartSize();
    }
}