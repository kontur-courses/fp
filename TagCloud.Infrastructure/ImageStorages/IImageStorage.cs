using System.Drawing;

public interface IImageStorage
{
    Result<None> Save(Bitmap image, string path);
}
