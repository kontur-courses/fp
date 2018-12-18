using System.Drawing;
using System.Windows.Forms;
using TagCloud.Data;

namespace TagCloud.Saver
{
    public class ClipboardImageSaver : IImageSaver
    {
        public Result<None> Save(Image image, string fileName)
        {
            return Result.OfAction(() => Clipboard.SetImage(image), "Unable to save to clipboard");
        }
    }
}