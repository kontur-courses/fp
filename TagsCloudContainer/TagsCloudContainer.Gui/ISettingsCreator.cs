using CSharpFunctionalExtensions;

namespace TagsCloudContainer.Gui;

public interface ISettingsCreator<TSetting>
{
    Result<TSetting> ShowCreate();
}