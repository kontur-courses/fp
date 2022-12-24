using ResultOfTask;

namespace TagsCloudContainer.Interfaces;

internal interface IDrawer
{
    public Result<string> DrawCloud(ICustomOptions options);
}