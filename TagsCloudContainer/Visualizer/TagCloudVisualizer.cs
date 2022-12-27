using System;
using System.Drawing;
using System.IO;
using TagsCloudContainer.CloudLayouter;

namespace TagsCloudContainer.Visualizer
{
    public class TagCloudVisualizer : IVisualizer
    {
        private readonly Bitmap bitmap;
        private readonly ICloudLayouter cloud;
        private readonly Graphics graphics;
        private readonly Options options;

        public TagCloudVisualizer(ICloudLayouter cloud, Options options)
        {
            this.cloud = cloud;
            this.options = options;
            bitmap = new Bitmap(options.Width, options.Height);
            graphics = Graphics.FromImage(bitmap);
        }

        public Result<None> Visualize()
        {
            var foregroundBrush = Result.Of(
                () => new SolidBrush(Color.FromName(options.ForegroundColor)));
            if (!foregroundBrush.IsSuccess)
                return Result.Fail<None>("Incorrect foreground color");
            var backgroundBrush = Result.Of(
                () => new SolidBrush(Color.FromName(options.BackgroundColor)));
            if (!backgroundBrush.IsSuccess)
                return Result.Fail<None>("Incorrect background color");

            graphics.FillRectangle(backgroundBrush.Value, 0, 0, options.Width, options.Height);
            foreach (var item in cloud.Items)
                graphics.DrawString(
                    item.Word, item.Font, foregroundBrush.Value, item.Rectangle);

            return Result.Ok();
        }

        public void Save()
        {
            var outputPath = Path.Combine(Program.ProjectPath, options.OutputFile);
            bitmap.Save(outputPath);
        }

        public SizeF MeasureString(string text, Font font)
        {
            return graphics.MeasureString(text, font);
        }

        public Result<Font> GetFontByWeightWord(int weight, int minWeight, int maxWeight)
        {
            var fontSize = Math.Max(options.MinFontSize,
                options.MaxFontSize * (weight - minWeight) / (maxWeight - minWeight));
            var fontFamily = Result.Of(() => new FontFamily(options.FontFamily));
            if (!fontFamily.IsSuccess)
                return Result.Fail<Font>("Incorrect Font Family");
            return new Font(fontFamily.Value, fontSize);
        }
    }
}