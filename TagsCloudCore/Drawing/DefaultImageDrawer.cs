using System.Drawing;
using System.Drawing.Imaging;
using TagsCloudCore.BuildingOptions;
using TagsCloudCore.Common.Enums;
using TagsCloudCore.Drawing.Colorers;
using TagsCloudCore.TagCloudForming;

namespace TagsCloudCore.Drawing;

public class DefaultImageDrawer : IImageDrawer
{
    private readonly IWordCloudDistributorProvider _distributedWordsProvider;
    private readonly DrawingOptions _drawingOptions;
    private readonly IEnumerable<IWordColorer> _wordColorers;

    public DefaultImageDrawer(IWordCloudDistributorProvider cloudDistributorProvider,
        IDrawingOptionsProvider drawingOptionsProvider, IEnumerable<IWordColorer> wordColorers)
    {
        _distributedWordsProvider = cloudDistributorProvider;
        _drawingOptions = drawingOptionsProvider.DrawingOptions;
        _wordColorers = wordColorers;
    }

    public Result<Bitmap> DrawImage(WordColorerAlgorithm colorerAlgorithm)
    {
        var wordsDistributionResult = _distributedWordsProvider.DistributeWords();
        if (!wordsDistributionResult.IsSuccess)
            return Result.Fail<Bitmap>($"Cannot draw the image. {wordsDistributionResult.Error}");
        
        var bitmap = new Bitmap(_drawingOptions.ImageSize.Width, _drawingOptions.ImageSize.Height);
        var offset = new Point(_drawingOptions.ImageSize.Width / 2, _drawingOptions.ImageSize.Height / 2);
        var graphics = Graphics.FromImage(bitmap);
        graphics.FillRectangle(new SolidBrush(_drawingOptions.BackgroundColor), 0, 0, bitmap.Width, bitmap.Height);
        
        var colorer = _wordColorers.SingleOrDefault(c => c.Match(colorerAlgorithm));
        if (colorer is null)
            return Result.Fail<Bitmap>("Couldn't find the required colorer algorithm among the registered ones.");
        
        foreach (var (value, word) in wordsDistributionResult.Value)
        {
            var sizeAdd = _drawingOptions.FrequencyScaling * (word.Frequency - 1);
            var newFont = new Font(_drawingOptions.Font.FontFamily, _drawingOptions.Font.Size + sizeAdd,
                _drawingOptions.Font.Style);
            
            var color = colorer.AlgorithmName == WordColorerAlgorithm.Default
                ? _drawingOptions.FontColor
                : colorer.GetWordColor(value, word.Frequency);

            var shiftedRectangle = word.Rectangle with {X = word.Rectangle.X + offset.X, Y = word.Rectangle.Y + offset.Y};
            if (!IsRectangleWithinBorders(shiftedRectangle, bitmap))
                return Result.Fail<Bitmap>("Tag cloud went beyond the image borders!");
            graphics.DrawString(value, newFont, new SolidBrush(color), shiftedRectangle);
        }

        return bitmap;
    }
    
    private static bool IsRectangleWithinBorders(Rectangle shiftedRectangle, Image bitmap)
        => shiftedRectangle is {Left: >= 0, Top: >= 0} 
               && shiftedRectangle.Bottom <= bitmap.Height 
               && shiftedRectangle.Right <= bitmap.Width;
    
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