using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using TagsCloudResult.UI;
using Color = SixLabors.ImageSharp.Color;
using PointF = SixLabors.ImageSharp.PointF;
using Rectangle = System.Drawing.Rectangle;
using Size = System.Drawing.Size;

namespace TagsCloudResult.Image;

public class ImageGenerator : IDisposable
{
    private readonly Image<Rgba32> image;
    private readonly string outputPath;
    private readonly int fontSize;
    private readonly FontFamily fontFamily;
    private readonly ImageEncoder encoder;
    private readonly Color scheme;

    public ImageGenerator(ApplicationArguments args)
    {
        fontFamily = FontFinder.TryGetFont(args).Unwrap();
        
        var format = args.Format switch
        {
            "bmp" => ImageEncodings.Bmp,
            "gif" => ImageEncodings.Gif,
            "jpg" => ImageEncodings.Jpg,
            "png" => ImageEncodings.Png,
            "tiff" => ImageEncodings.Tiff,
            _ => ImageEncodings.Jpg
        };

        image = new Image<Rgba32>(args.Resolution[0], args.Resolution[1]);
        outputPath = args.Output + "." + format.ext;
        encoder = format.encoding;
        scheme = Color.FromRgba(
            (byte)args.Scheme[0],
            (byte)args.Scheme[1],
            (byte)args.Scheme[2],
            (byte)args.Scheme[3]
        );
        fontSize = args.FontSize;

        SetBackground(Color.FromRgb(
            (byte)args.Background[0],
            (byte)args.Background[1],
            (byte)args.Background[2])
        );
    }

    private Font FontCreator(int size)
    {
        return fontFamily.CreateFont(size, FontStyle.Italic);
    }

    private void SetBackground(Color color)
    {
        image.Mutate(x => x.Fill(color));
    }

    private void DrawWord(string word, int frequency, Rectangle rectangle)
    {
        image.Mutate(x => x.DrawText(
            word, FontCreator(fontSize + frequency), scheme, new PointF(rectangle.X, rectangle.Y))
        );
    }

    public MyResult DrawLayout(IEnumerable<Rectangle> rectangles)
    {
        foreach (var tmpRect in rectangles)
        {
            var rectangle = new RectangleF(tmpRect.X, tmpRect.Y, tmpRect.Width, tmpRect.Height);
            image.Mutate(x => x.Draw(Color.Red, 2f, rectangle));
        }

        var saveResult = MyResult.Try<object>(() =>
        {
            image.Save(outputPath, encoder);
            return null!;
        });

        return saveResult.IsErr
            ? MyResult.Err($"\"SixLabors.ImageSharp library exception:\\n{saveResult.UnwrapErr()}\"")
            : MyResult.Ok();
    }

    public MyResult DrawTagCloud(List<(string word, int frequency, Rectangle outline)> wordsFrequenciesOutline)
    {
        foreach (var wordFrequencyOutline in wordsFrequenciesOutline)
            DrawWord(wordFrequencyOutline.word, wordFrequencyOutline.frequency, wordFrequencyOutline.outline);
        
        var saveResult = MyResult.Try<object>(() =>
        {
            image.Save(outputPath, encoder);
            return null!;
        });

        return saveResult.IsErr
            ? MyResult.Err($"\"SixLabors.ImageSharp library exception:\\n{saveResult.UnwrapErr()}\"")
            : MyResult.Ok();
    }

    public Size GetOuterRectangle(string word, int frequency)
    {
        var textOption = new TextOptions(FontCreator(fontSize + frequency));
        var size = TextMeasurer.MeasureSize(word, textOption);

        return new Size((int)size.Width + fontSize / 3, (int)size.Height + fontSize / 3);
    }

    public MyResult RectangleOutOfResolution(Rectangle rectangle)
    {
        var tmp = new Rectangle(rectangle.Location, rectangle.Size);

        rectangle.Intersect(new Rectangle(0, 0, image.Width, image.Height));

        return tmp.Equals(rectangle) ? MyResult.Ok() : MyResult.Err("Tag cloud out of image resolution");
    }

    public void Dispose()
    {
        image.Dispose();
    }
}