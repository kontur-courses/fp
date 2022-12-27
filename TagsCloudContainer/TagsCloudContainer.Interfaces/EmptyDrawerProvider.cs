using System.Drawing;
using CSharpFunctionalExtensions;

namespace TagsCloudContainer.Interfaces;

public class EmptyDrawerProvider : IDrawerProvider
{
    public Result<IDrawer> Provide(ILayouterAlgorithmProvider layouterAlgorithmProvider, Graphics graphics)
    {
        return Result.Failure<IDrawer>(nameof(EmptyDrawerProvider));
    }
}