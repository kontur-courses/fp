using System.Collections.Generic;
using System.Drawing;

namespace TagCloud.settings
{
    public interface IDrawSettings
    {
        List<Color> GetInnerColors(); 
        Color GetBackgroundColor();
        Size GetSize();
    }
}