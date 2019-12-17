using System.Drawing;

namespace TagsCloudGenerator.Interfaces
{
    public interface IPainterSettings : IResettable
    {
        string[] ColorsNames { get; set; }
        string BackgroundColorName { get; set; }
    }
}