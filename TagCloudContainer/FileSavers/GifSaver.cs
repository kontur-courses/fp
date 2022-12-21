using System.Drawing;
using System.Drawing.Imaging;

namespace TagCloudContainer.FileSavers
{
    public class GifSaver : IFileSaver
    {
        public void SaveCanvas(string pathToSave, Bitmap canvas) => canvas.Save(pathToSave + ".gif", ImageFormat.Gif);
    }
}