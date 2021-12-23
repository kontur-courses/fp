using System.Drawing;
using TagsCloudContainerCore.Result;

namespace TagsCloudContainerCore.CircularLayouter;

public interface ILayouter
{
    // ReSharper disable once UnusedMember.Global
    Result<Rectangle> PutNextRectangle(Size size);
}