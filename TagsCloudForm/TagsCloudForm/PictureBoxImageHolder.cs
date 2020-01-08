using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using TagsCloudForm.Common;

namespace TagsCloudForm
{
    public class PictureBoxImageHolder : PictureBox, IImageHolder
    {
        public Size GetImageSize()
        {
            FailIfNotInitialized();
            return Image.Size;
        }

        public IGraphicDrawer StartDrawing()
        {
            FailIfNotInitialized();
            return new GraphicDrawer(Image);
        }

        private void FailIfNotInitialized()
        {
            if (Image == null)
                throw new InvalidOperationException("Call PictureBoxImageHolder.RecreateImage before other method call!");
        }

        public void UpdateUi()
        {
            Refresh();
            Application.DoEvents();
        }

        public void RecreateImage(ImageSettings imageSettings)
        {
            Image?.Dispose();
            Image = new Bitmap(imageSettings.Width, imageSettings.Height, PixelFormat.Format24bppRgb);
        }

        public void SaveImage(string fileName)
        {
            FailIfNotInitialized();
            Image.Save(fileName);
        }


        protected override void Dispose(bool disposing)
        {
            Image?.Dispose();
            base.Dispose(disposing);
        }
    }
}