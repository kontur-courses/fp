using Results;

namespace TagsCloudVisualization.Settings;

public interface ISettings
{
    Result<bool> Check();
}