using System.Drawing;
using System.Drawing.Imaging;

namespace TagCloudContainer.FileSavers
{
    public class PngSaver : IFileSaver
    {
        public void SaveCanvas(string pathToSave, Bitmap canvas) => canvas.Save(pathToSave + ".png", ImageFormat.Png);
    }
}