using System.Drawing;
using System.Drawing.Imaging;

namespace TagCloudContainer.FileSavers
{
    public class BmpSaver : IFileSaver
    {
        public void SaveCanvas(string pathToSave, Bitmap canvas) => canvas.Save(pathToSave + ".bmp", ImageFormat.Bmp);
    }
}