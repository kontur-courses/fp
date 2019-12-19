using System.Drawing;
using TagsCloudContainer.Data;
using TagsCloudContainer.Functional;

namespace TagsCloudContainer.Visualization.Measurers
{
    public interface IWordMeasurer
    {
        Result<(Font font, Size size)> Measure(Word word);
    }
}