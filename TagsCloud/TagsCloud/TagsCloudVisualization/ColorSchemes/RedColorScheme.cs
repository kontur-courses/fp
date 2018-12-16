using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagsCloud.ErrorHandling;

namespace TagsCloud.TagsCloudVisualization.ColorSchemes
{
    public class RedColorScheme: IColorScheme
    {
        private  int maxDAlpha = 200;
        public Result<Color> DefineColor(int frequency)
        {
            if (frequency <= 0)
                return Result.Fail<Color>($"Wrong frequency '{frequency}'");
            return Color.FromArgb(255 - maxDAlpha/frequency, Color.Red);
        }
    }
}
