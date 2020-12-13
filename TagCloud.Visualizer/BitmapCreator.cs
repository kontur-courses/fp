using System.Collections.Generic;
using System.Drawing;
using TagCloud.ExceptionHandler;

namespace TagCloud.Visualizer
{
    public static class BitmapCreator
    {
        internal static Result<Bitmap> DrawBitmap(Result<List<RectangleWithWord>> rectanglesWithWordsResult, ImageOptions opts)
        {
            var bitmap = new Bitmap(opts.ImageWidth, opts.ImageHeight);
            using var graph = Graphics.FromImage(bitmap);
            graph.Clear(Color.White);
            var brush = (Brush) typeof(Brushes).GetProperty($"{opts.ColorName}")?.GetValue(null);
            if (!rectanglesWithWordsResult.IsSuccess)
            {
                return Result.Fail<Bitmap>(rectanglesWithWordsResult.Error);
            }
            foreach (var rectangleWithWord in rectanglesWithWordsResult.GetValueOrThrow())
            {
                using var font = new Font(opts.FontName, opts.FontSize * (float) rectangleWithWord.Word.Weight);
                graph.DrawString(rectangleWithWord.Word.Value,
                    font,
                    brush!,
                    rectangleWithWord.Rectangle);
            }

            return bitmap.AsResult();
        }
    }
}