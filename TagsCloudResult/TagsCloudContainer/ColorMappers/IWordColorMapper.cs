using System.Collections.Generic;
using System.Drawing;
using TagsCloudContainer.DependencyInjection;
using TagsCloudContainer.Layout;
using TagsCloudContainer.Results;

namespace TagsCloudContainer.ColorMappers
{
    public interface IWordColorMapper : IService<WordColorMapperType>
    {
        public Result<Dictionary<WordLayout, Color>> GetColorMap(CloudLayout layout);
    }
}