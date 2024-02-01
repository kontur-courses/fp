using System.Drawing;
using System.Drawing.Imaging;
using TagsCloudCore.BuildingOptions;
using TagsCloudCore.Common;
using TagsCloudCore.Common.Enums;
using TagsCloudCore.Drawing.Colorers;
using TagsCloudCore.TagCloudForming;

namespace TagsCloudCore.Drawing;

public class DefaultImageDrawer : IImageDrawer
{
    private readonly IReadOnlyDictionary<string, WordData> _distributedWords;
    private readonly DrawingOptions _drawingOptions;
    private readonly IEnumerable<IWordColorer> _wordColorers;

    public DefaultImageDrawer(IWordCloudDistributorProvider cloudDistributorProvider,
        IDrawingOptionsProvider drawingOptionsProvider, IEnumerable<IWordColorer> wordColorers)
    {
        _distributedWords = cloudDistributorProvider.DistributedWords;
        _drawingOptions = drawingOptionsProvider.DrawingOptions;
        _wordColorers = wordColorers;
    }

    public Bitmap DrawImage(WordColorerAlgorithm colorerAlgorithm)
    {
        var bitmap = new Bitmap(_drawingOptions.ImageSize.Width, _drawingOptions.ImageSize.Height);
        var offset = new Point(_drawingOptions.ImageSize.Width / 2, _drawingOptions.ImageSize.Height / 2);
        var graphics = Graphics.FromImage(bitmap);
        graphics.FillRectangle(new SolidBrush(_drawingOptions.BackgroundColor), 0, 0, bitmap.Width, bitmap.Height);

        foreach (var (value, word) in _distributedWords)
        {
            var sizeAdd = _drawingOptions.FrequencyScaling * (word.Frequency - 1);
            var newFont = new Font(_drawingOptions.Font.FontFamily, _drawingOptions.Font.Size + sizeAdd,
                _drawingOptions.Font.Style);

            var colorer = _wordColorers.SingleOrDefault(c => c.Match(colorerAlgorithm));

            var color = colorer!.AlgorithmName == WordColorerAlgorithm.Default
                ? _drawingOptions.FontColor
                : colorer.GetWordColor(value, word.Frequency);

            graphics.DrawString(value, newFont, new SolidBrush(color),
                word.Rectangle with {X = word.Rectangle.X + offset.X, Y = word.Rectangle.Y + offset.Y});
        }

        return bitmap;
    }

    public static Result<None> SaveImage(Bitmap bitmap, string dirPath, string filename, ImageFormat imageFormat)
    {
        if (string.IsNullOrWhiteSpace(filename) || filename.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0)
            return Result.Fail<None>($"The provided filename {filename} is not valid.");

        return Result
            .OfAction(() => Directory.CreateDirectory(dirPath),
                $"The provided directory path {dirPath} is not valid.")
            .Then(_ => bitmap.Save(Path.Combine(dirPath, filename), imageFormat));
    }
}