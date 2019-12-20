using System.Collections.Generic;
using TagsCloudContainer.RectangleTranslation;
using TagsCloudContainer.ResultInfrastructure;

namespace TagsCloudContainer.Visualization.Interfaces
{
    public interface IWordLayouter
    {
        Result<List<WordRectangle>> LayoutWords(IEnumerable<SizedWord> sizedWords);
    }
}