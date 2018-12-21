using System;
using System.Drawing;
using System.Threading;

namespace TagsCloudResult
{
    public class DrawSettings<T>
    {
        private Size imageSize;
        private string fontName;
        private Func<IItemToDraw<T>, Color> itemPainter;
        private string filePath;
        private ImageFileFormat imageFileFormat;

        public DrawSettings()
        {
            InitDefaultSettings();
        }

        public DrawSettings(string filePath)
        {
            InitDefaultSettings();
            this.filePath = filePath;
        }

        private void InitDefaultSettings()
        {
            imageSize = new Size(1000, 500);
            fontName = "Times new Roman";
            itemPainter = i => TakeRandomColor();
            filePath = "result";
            imageFileFormat = ImageFileFormat.Png;
        }

        public Result<string> GetFullFileName()
        {
            return string.IsNullOrEmpty(filePath) 
                ? new Result<string>("file path empty or null") : (filePath + "." + imageFileFormat.ToString().ToLower()).AsResult();
        }

        public Result<None> SetImageSize(Size newImageSize)
        {
            if (newImageSize.Width <= 0 || newImageSize.Height <= 0)
                return new Result<None>("both image size parameters should be positive");

            imageSize = newImageSize;

            return new Result<None>();
        }

        public Result<None> SetFontName(string newFontName)
        {
            if (string.IsNullOrEmpty(newFontName))
                return new Result<None>("font name shouldn't be null or empty");

            fontName = newFontName;

            return new Result<None>();
        }

        public Result<None> SetItemPainter(Func<IItemToDraw<T>, Color> newItemPainter)
        {
            if (newItemPainter == null)
                return new Result<None>("item painter func shouldn't be null");
            try
            {
                itemPainter = newItemPainter;
                return new Result<None>();
            }
            catch (Exception e)
            {
                return new Result<None>(e.Message);
            }
        }

        public Result<None> SetFilePath(string newFilePath)
        {
            if (string.IsNullOrEmpty(newFilePath))
                return new Result<None>("file path shouldn't be null or empty");

            filePath = newFilePath;

            return new Result<None>();
        }

        public void SetImageFileFormat(ImageFileFormat newImageFileFormat)
        {
            imageFileFormat = newImageFileFormat;
        }

        public Size GetImageSize()
        {
            return imageSize;
        }

        public string GetFontName()
        {
            return fontName;
        }

        public SolidBrush GetBrush(IItemToDraw<T> item)
        {
            return new SolidBrush(itemPainter(item));
        }

        public Result<string> GetFilePath()
        {
            return string.IsNullOrEmpty(filePath) ?
                new Result<string>("file path null or empty") : new Result<string>(null, filePath);
        }

        public ImageFileFormat GetImageFileFormat()
        {
            return imageFileFormat;
        }

        private static Color TakeRandomColor()
        {
            var rnd = new Random();
            Thread.Sleep(20);
            var color = Color.FromArgb(rnd.Next());

            switch (rnd.Next() % 4)
            {
                case 0:
                    color = Color.Green;
                    break;
                case 1:
                    color = Color.Red;
                    break;
                case 2:
                    color = Color.Gold;
                    break;
                case 3:
                    color = Color.Aqua;
                    break;
                default:
                    color = Color.BlueViolet;
                    break;
            }

            return color;
        }
    }
}
