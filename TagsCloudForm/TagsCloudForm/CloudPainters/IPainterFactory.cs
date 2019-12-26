using TagsCloudForm.Common;

namespace TagsCloudForm.CloudPainters
{
    public interface IPainterFactory
    {
        Result<ICloudPainter> Create();
    }
}
