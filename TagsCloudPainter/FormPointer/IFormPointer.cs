using ResultLibrary;
using System.Drawing;

namespace TagsCloudPainter.FormPointer;

public interface IFormPointer : IResetable
{
    Result<Point> GetNextPoint();
}