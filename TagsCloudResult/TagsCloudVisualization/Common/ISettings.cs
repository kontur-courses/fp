using TagsCloudVisualization.Common.ResultOf;

namespace TagsCloudVisualization.Common;

public interface ISettings<TSettings> where TSettings : ISettings<TSettings>
{
    public Result<TSettings> Validate();
}
