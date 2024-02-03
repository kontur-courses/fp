namespace TagCloud.CloudDrawers;

public interface ICloudDrawer
{
    Result<None> Draw(List<TextRectangle> rectangle);
}