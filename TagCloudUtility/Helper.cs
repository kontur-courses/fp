using System.Drawing.Imaging;
using System.IO;
using TagCloud.Utility.Container;

namespace TagCloud.Utility
{
    public static class Helper
    {
        public static string GetExtension(string path)
        {
            return Path.GetExtension(path);
        }

        public static Result<Options> CheckPaths(Options options)
        {
            if (!File.Exists(options.PathToWords))
                return Result.Fail<Options>(
                    $"File {options.PathToWords} doesn't exists!");
            if (!Path.HasExtension(options.PathToWords))
                return Result.Fail<Options>(
                    $"Path to words should contain file type, but was {options.PathToWords}");

            if (!Path.HasExtension(options.PathToPicture))
                return Result.Fail<Options>(
                    $"Path to picture should contain picture type, but was {options.PathToPicture}");


            if (options.PathToTags != null)
            {
                if (!File.Exists(options.PathToTags))
                    return Result.Fail<Options>($"File {options.PathToTags} doesn't exists!");
                if (!Path.HasExtension(options.PathToTags))
                    return Result.Fail<Options>($"Path to tags should contain file type, but was {options.PathToTags}");
            }

            if (options.PathToStopWords != null)
            {
                if (!File.Exists(options.PathToStopWords))
                    return Result.Fail<Options>($"File {options.PathToStopWords} doesn't exists!");
                if (!Path.HasExtension(options.PathToStopWords))
                    return Result.Fail<Options>(
                        $"Path to stopwords should contain file type, but was {options.PathToStopWords}");
            }

            return options.AsResult();
        }

        public static string GetPath(string path)
        {
            return Path.Combine(Directory.GetCurrentDirectory(), path);
        }

        public static Result<ImageFormat> GetImageFormat(string fileName)
        {
            var extension = Path.GetExtension(fileName);
            if (string.IsNullOrEmpty(extension))
                return Result.Fail<ImageFormat>(
                    $"Unable to determine file extension for {fileName}");

            switch (extension.ToLower())
            {
                case @".bmp":
                    return ImageFormat.Bmp;

                case @".gif":
                    return ImageFormat.Gif;

                case @".ico":
                    return ImageFormat.Icon;

                case @".jpg":
                case @".jpeg":
                    return ImageFormat.Jpeg;

                case @".png":
                    return ImageFormat.Png;

                case @".tif":
                case @".tiff":
                    return ImageFormat.Tiff;

                case @".wmf":
                    return ImageFormat.Wmf;

                default:
                    return Result.Fail<ImageFormat>(
                        $"Unable to determine picture extension for file: {fileName}");
            }
        }
    }
}