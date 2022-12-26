using System.Drawing;
using CSharpFunctionalExtensions;

namespace TagsCloudContainer.Interfaces;

public interface ILayouterAlgorithm
{
    Result<Rectangle> PutNextRectangle(Size rectangleSize);
}