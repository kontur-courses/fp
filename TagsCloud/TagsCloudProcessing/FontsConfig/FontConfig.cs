using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagsCloud.TagsCloudProcessing.FontsConfig
{
    public class FontConfig : IFontConfig
    {
        public FontFamily FontFamily { get; }
        public float Size { get; }

        public FontConfig(FontFamily fontFamily, float size)
        {
            FontFamily = fontFamily;
            Size = size;
        }
    }
}
