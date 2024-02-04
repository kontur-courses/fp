using System.Drawing;
using ResultLibrary;

namespace TagsCloudPainter.FormPointer;

public interface IFormPointer : IResetable
{
    Result<Point> GetNextPoint();
}