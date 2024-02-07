using System.Drawing;

public interface ITagCloud
{
    Result<Bitmap> CreateCloud(string text);
}