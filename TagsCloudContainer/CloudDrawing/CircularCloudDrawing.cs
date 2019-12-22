using System;
using System.Collections.Generic;
using System.Drawing;
using CloudLayouter;
using ResultOf;

namespace CloudDrawing
{
    public class CircularCloudDrawing : ICircularCloudDrawing
    {
        private ICloudLayouter layouter;
        private Bitmap bitmap;
        private Graphics graphics;

        public CircularCloudDrawing(ICloudLayouter cloudLayouter)
        {
            layouter = cloudLayouter;
        }

        public Result<None> SetOptions(ImageSettings imageSettings)
        {
            if (imageSettings.Size.Height <= 0 || imageSettings.Size.Height <= 0)
                return Result.Fail<None>("Ширина или высота была меньше или равна нулю");
            bitmap = new Bitmap(imageSettings.Size.Width, imageSettings.Size.Height);
            graphics = Graphics.FromImage(bitmap);

            layouter.SetCenter(new Point(imageSettings.Size.Width / 2, imageSettings.Size.Height / 2));
            graphics.Clear(imageSettings.Background);
            return Result.Ok();
        }

        public Result<None> DrawWords(IEnumerable<(string, int)> wordsFontSize, WordDrawSettings settings)
        {
            foreach (var (word, fontSize) in wordsFontSize)
            {
                var rectangle = 
                    DrawWord(word, new Font(settings.FamilyName, fontSize), settings.Brush, settings.StringFormat)
                    .Apply(r =>
                    {
                        if (settings.HaveDelineation) DrawRectangle(r);
                    });
                if (!rectangle.IsSuccess)
                    return Result.Fail<None>(rectangle.Error);
            }
            return Result.Ok();
        }

        private Result<Rectangle> DrawWord(string word, Font font, Brush brush, StringFormat stringFormat)
        {
            var stringSize = (graphics.MeasureString(word, font) + new SizeF(1, 1)).ToSize();
            var stringRectangle = layouter.PutNextRectangle(stringSize);
            graphics.DrawString(word, font, brush, stringRectangle, stringFormat);
            return Result.Validate(stringRectangle, r => graphics.IsVisible(r),
                "Слово вышло за пределы области видимости");
        }

        private void DrawRectangle(Rectangle rectangle)
        {
            graphics.DrawRectangle(new Pen(Color.Black), rectangle);
        }

        public Result<None> SaveImage(string filename)
        {
            return filename.AsResult()
                .Then(bitmap.Save)
                .RefineError("Не удается сохранить в указанный файл");
        }
    }
}