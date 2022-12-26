using CSharpFunctionalExtensions;

namespace TagsCloudContainer.Interfaces;

public interface IDrawer
{
    Result DrawCloud(IEnumerable<CloudWord> cloudWords);
}