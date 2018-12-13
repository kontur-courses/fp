using System;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;
using TagCloud.ExceptionHandler;
using TagCloud.Forms;

namespace TagCloud
{
    public class SaveImageAction : IUiAction
    {
        private readonly ImageBox imageBox;
        private readonly IExceptionHandler exceptionHandler;

        public SaveImageAction(ImageBox imageBox, IExceptionHandler exceptionHandler)
        {
            this.imageBox = imageBox;
            this.exceptionHandler = exceptionHandler;
        }

        public string Category => "Image";
        public string Name => "Save";
        public string Description => "Save image";

        public void Perform()
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.CheckFileExists = false;
                openFileDialog.DefaultExt = "png";
                openFileDialog.FileName = "TagCloud";
                openFileDialog.Filter = "Bitmap (*.bmp)|*.bmp|PNG (*.png)|*.png|Gif (*.gif)|*.gif |JPEG (*.jpeg)|*.jpeg  |All files (*.*)|*.*";
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    Result.Of(() => GetImageFormat(openFileDialog.FileName))
                        .Then(format => imageBox.SaveImage(openFileDialog.FileName, format))
                        .RefineError("Failed, tryng to save image")
                        .OnFail(exceptionHandler.HandleException);
//                    var currentExtension = GetImageFormat(openFileDialog.FileName);
//                    imageBox.SaveImage(openFileDialog.FileName, currentExtension);
                }
            }

        }
        
        private ImageFormat GetImageFormat(string fileName)
        {
            string extension = Path.GetExtension(fileName);
            if (string.IsNullOrEmpty(extension))
                return ImageFormat.Png;

            switch (extension.ToLower())
            {
                case @".bmp":
                    return ImageFormat.Bmp;

                case @".gif":
                    return ImageFormat.Gif;

                case @".jpg":
                case @".jpeg":
                    return ImageFormat.Jpeg;

                case @".png":
                    return ImageFormat.Png;

                default:
                    return ImageFormat.Png;
            }
        }
    }
}