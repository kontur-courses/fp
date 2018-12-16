using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagsCloud.TagsCloudVisualization.ColorSchemes
{
    public interface IColorScheme
    {
        Result<Color> DefineColor(int frequency);
    }
}
