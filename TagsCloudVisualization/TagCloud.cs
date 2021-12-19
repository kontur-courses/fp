using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using TagsCloudVisualization.Infrastructure;

namespace TagsCloudVisualization
{
    public class TagCloud
    {
        private readonly FileReader fileReader;
        private readonly TokenGenerator tokenGenerator;
        private readonly TagCloudMaker tagCloudMaker;
        private readonly TagCloudVisualiser tagCloudVisualiser;

        public TagCloud(FileReader fileReader, TokenGenerator tokenGenerator,
            TagCloudMaker tagCloudMaker, TagCloudVisualiser tagCloudVisualiser)
        {
            this.fileReader = fileReader;
            this.tagCloudMaker = tagCloudMaker;
            this.tagCloudVisualiser = tagCloudVisualiser;
            this.tokenGenerator = tokenGenerator;
        }

        public Result<None> CreateTagCloudFromFile(string sourcePath, string resultPath, Font font,
            Color background, int maxTagCount, Size resolution)
        {
            var source = ParseSource(sourcePath);
            if (!source.IsSuccess)
                return Result.Fail<None>(source.Error);
            var tokensResult = fileReader.ReadFile(source.Value)
                .Then(text => tokenGenerator.GetTokens(text, maxTagCount));
            if (!tokensResult.IsSuccess)
                return Result.Fail<None>(tokensResult.Error);
            var tags = tagCloudMaker.CreateTagCloud(tokensResult.Value, font);
            if (tags.Length == 0)
                return Result.Fail<None>("Zero tags found");
            if (resolution.Height < 1 || resolution.Width < 1)
                return Result.Fail<None>("Resolution must be positive");
            var image = tagCloudVisualiser.Render(tags, resolution, background);
            var format = ParseImageFormat(resultPath);
            return format.IsSuccess ?  Result.Of(() => image.Save(resultPath, format.Value)) :
                Result.Fail<None>(format.Error);
        }

        public Result<None> CreateTagCloudFromFile(string sourcePath, string resultPath, string fontName,
            string background, int maxTagCount, int width, int height)
        {
            var resolution = ParseResolution(width, height);
            if (!resolution.IsSuccess)
                return Result.Fail<None>(resolution.Error);
            var font = ParseFont(fontName);
            if (!font.IsSuccess)
                return Result.Fail<None>(font.Error);
            var back = ParseColor(background);
            if (!back.IsSuccess)
                return Result.Fail<None>(back.Error);
            return CreateTagCloudFromFile(sourcePath, resultPath, font.Value, back.Value,
                maxTagCount, resolution.Value);
        }
        
        public static Result<Font> ParseFont(string fontName)
        {
            var font = new Font(fontName, 10);
            return font.Name == fontName  ? font.AsResult() : Result.Fail<Font>("Unknown Font " + fontName);
        }

        public static Result<Color> ParseColor(string colorName)
        {
            return Enum.TryParse(colorName, out KnownColor color) ? 
                Color.FromKnownColor(color).AsResult() : Result.Fail<Color>("Unknown color " + colorName);
        }

        public static Result<FileInfo> ParseSource(string sourcePath)
        {
            var source = new FileInfo(sourcePath);
            return source.Exists ? source.AsResult() : Result.Fail<FileInfo>("Source file not found");
        }

        public static Result<Size> ParseResolution(int width, int height)
        {
            return height > 0 && width > 0 ? new Size(width, height).AsResult() : 
                Result.Fail<Size>("Resolution must be positive");
        }
        
        public static Result<ImageFormat> ParseImageFormat(string path)
        {
            var extension = Path.GetExtension(path).TrimStart('.');
            extension = extension.ToLower();
            if (extension == "jpg")
                return ImageFormat.Jpeg.AsResult();
            extension = extension[0].ToString().ToUpper() + extension[1..];
            var type = typeof(ImageFormat);
            var prop = type.GetProperty(extension);
            if (prop is null || prop.PropertyType != type)
                return Result.Fail<ImageFormat>("Unknown image format");
            return Result.Ok((ImageFormat)prop.GetValue(new object()));
        }
    }
}