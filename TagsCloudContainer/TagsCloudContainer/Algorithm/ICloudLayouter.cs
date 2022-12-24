using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagsCloudContainer.Infrastructure;

namespace TagsCloudContainer.Algorithm
{
    public interface ICloudLayouter
    {
        public Result<List<(Rectangle rectangle, string text)>> FindRectanglesPositions(int imgWidth, int imgHeight,
            Dictionary<string, int> wordsCount);
    }
}
