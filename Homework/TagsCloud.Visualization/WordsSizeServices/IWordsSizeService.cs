using System.Drawing;
using TagsCloud.Visualization.FontFactories;
using TagsCloud.Visualization.Models;

namespace TagsCloud.Visualization.WordsSizeServices
{
    public interface IWordsSizeService
    {
        Size CalculateSize(Word word, FontDecorator font);
    }
}