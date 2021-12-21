using System.Drawing;

namespace TagsCloudContainerCore.CircularLayouter;

public interface ILayouter
{
    // ReSharper disable once UnusedMember.Global
    Rectangle PutNextRectangle(Size size);
}