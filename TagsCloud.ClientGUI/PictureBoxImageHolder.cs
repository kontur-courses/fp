using System.Drawing;
using System.Windows.Forms;
using TagsCloud.ResultPattern;
using TagsCloud.Visualization;

namespace TagsCloud.ClientGUI
{
    public class PictureBoxImageHolder : PictureBox, IImageHolder
    {
        public Result<Size> GetImageSize()
        {
            return Image.AsResult()
                .Then(x => x.Size)
                .ReplaceError(x => "Call PictureBoxImageHolder.RecreateImage before other method call!");
        }

        public Result<Graphics> StartDrawing()
        {
            return Image.AsResult()
                .Then(Graphics.FromImage)
                .ReplaceError(x => "Call PictureBoxImageHolder.RecreateImage before other method call!");
        }

        public void RecreateImage(ImageSettings imageSettings)
        {
            Image?.Dispose();
            Image = new Bitmap(imageSettings.Width, imageSettings.Height);
        }

        public Result<None> SaveImage(string fileName)
        {
            return Image.AsResult()
                .Then(x => x.Save(fileName))
                .ReplaceError(x => "Call PictureBoxImageHolder.RecreateImage before other method call!");
        }
    }
}