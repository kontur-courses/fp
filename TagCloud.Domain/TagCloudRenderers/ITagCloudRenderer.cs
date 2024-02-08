using System.Drawing;

public interface ITagCloudRenderer
{
    Result<Bitmap> Render(WordLayout[] wordLayouts);

    Size GetStringSize(string str, int fontSize);
}
