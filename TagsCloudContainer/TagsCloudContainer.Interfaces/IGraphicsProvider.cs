using System.Drawing;
using CSharpFunctionalExtensions;

namespace TagsCloudContainer.Interfaces;

public interface IGraphicsProvider
{
    Result<Graphics> Create();

    Result Commit();
}