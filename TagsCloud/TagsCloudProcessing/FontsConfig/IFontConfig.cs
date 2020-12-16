using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagsCloud.TagsCloudProcessing.FontsConfig
{
    public interface IFontConfig
    {
        FontFamily FontFamily { get; }
        float Size { get; }
    }
}
