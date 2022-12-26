using System.Drawing;
using CSharpFunctionalExtensions;

namespace TagsCloudContainer.Interfaces;

public interface IDrawerProvider
{
    Result<IDrawer> Provide(ILayouterAlgorithmProvider layouterAlgorithmProvider, Graphics graphics);
}