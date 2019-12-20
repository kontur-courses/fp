using ResultLogic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System;

namespace TagCloud.TagCloudVisualisation.Canvas
{
    public class TagCloudCanvas : Canvas
    {
        public TagCloudCanvas(int width, int height) : base(width, height) {}

        public override Result<None> Draw(Rectangle rectangle, Brush brush)
        {
            return Result.OfAction(() => Graphics.FillRectangle(brush, rectangle),
                new Exception($"Не удалось отрисовать прямоуголник: {rectangle}"));
        }

        public override Result<None> Draw(string word, Font font, RectangleF rectangleF, Brush brush)
        {
            return Result.OfAction(() => Graphics.DrawString(word, font, brush, rectangleF),
                new Exception("Не удалось отрисовать текст с заданными параметрами:" + Environment.NewLine 
                + $"word : {word}" + Environment.NewLine 
                + $"font : {font}" + Environment.NewLine
                + $"rectangleF : {rectangleF}" + Environment.NewLine
                + $"brush : {brush}"));
        }

        public Result<None> Save(string fileName)
        {
            return Result.OfAction(() => Bitmap.Save(fileName + ".png"), 
                new Exception($"Не удалось сохранить файл изображения"));
        }

        public override Result<None> Save(string directoryPath, string fileName)
        {
            var pathToFile = Path.Combine(directoryPath, fileName + ".png");
            return Result.OfAction(() => Bitmap.Save(pathToFile, ImageFormat.Png), 
                new Exception($"Не удалось сохранить файл по указанному пути {pathToFile}"));
        }
    }
}
