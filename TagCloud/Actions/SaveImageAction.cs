using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;
using TagCloud.ExceptionHandler;
using TagCloud.Forms;

namespace TagCloud
{
    public class SaveImageAction : IUiAction
    {
        private readonly IExceptionHandler exceptionHandler;
        private readonly ImageBox imageBox;

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
            using (var saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.CheckFileExists = false;
                saveFileDialog.DefaultExt = "png";
                saveFileDialog.FileName = "TagCloud";
                saveFileDialog.Filter =
                    "Bitmap (*.bmp)|*.bmp|PNG (*.png)|*.png|Gif (*.gif)|*.gif |JPEG (*.jpeg)|*.jpeg  |All files (*.*)|*.*";
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    Result.Of(() => GetImageFormat(saveFileDialog.FileName))
                        .Then(format => imageBox.SaveImage(saveFileDialog.FileName, format))
                        .RefineError("Failed, trying to save image")
                        .OnFail(exceptionHandler.HandleException);
            }
        }

        private ImageFormat GetImageFormat(string fileName)
        {
            var extension = Path.GetExtension(fileName);
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