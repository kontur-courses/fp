using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using TagCloud.TagCloudVisualisation.Canvas;
using TagCloud.TagCloudVisualisation.TagCloudLayouter;
using TagCloud.TextPreprocessor.Core;
using ResultLogic;

namespace TagCloud.TagCloudPainter
{
    public class Painter : ITagCloudPainter
    {
        private readonly LayouterFactory layouterFactory;
        private readonly PainterConfig painterConfig;
        private readonly Random random;

        public Painter(LayouterFactory layouterFactory, PainterConfig config)
        {
            this.layouterFactory = layouterFactory;
            painterConfig = config;
            random = new Random();
        }
        
        public Result<None> Draw(IEnumerable<TagInfo> tagInfos)
        {
            if (painterConfig.ImageWidth < 0 || painterConfig.ImageHeight < 0)
                return Result.Fail<None>(new Exception($"Не верно заданы размеры изображение ширина:{painterConfig.ImageWidth} высота: {painterConfig.ImageHeight}"));

            var canvas = new TagCloudCanvas(painterConfig.ImageWidth, painterConfig.ImageHeight);
            
            return DrawBackground(canvas)
                .Then(x => DrawTags(canvas, tagInfos))
                .Then(x => canvas.Save(painterConfig.PathForSave, painterConfig.ImageName));
        } 

        private int GetTagFontSize(TagInfo tagInfo, PainterConfig config, int maxFrequency, int minFrequency)
        {
            if (maxFrequency == minFrequency)
                return config.MaxFontSize;

            var fontSize = config.MinFontSize + ((config.MaxFontSize - config.MinFontSize) * tagInfo.Frequency) /
                           (maxFrequency - minFrequency);
            return fontSize;
        }

        private Size GetTagSize(TagInfo tagInfo, int fontSize, FontFamily family)
        {
            var font = new Font(family, fontSize);
            return TextRenderer.MeasureText(tagInfo.Tag.Content, font);
        }
        
        private Brush GetRandomBrush(Color[] pallet)
        {
            var color = pallet[random.Next(0, pallet.Length)];

            return new SolidBrush(color);
        }

        private Result<None> DrawBackground(Canvas canvas)
        {
            return canvas.Draw(
                new Rectangle(new Point(0,0), new Size(painterConfig.ImageWidth, painterConfig.ImageHeight)), 
                new SolidBrush(painterConfig.BackgroundColor));
        }

        private Result<None> DrawTags(Canvas canvas, IEnumerable<TagInfo> tagInfos)
        {
            var layouter = layouterFactory
                .GetCircularLayouter(painterConfig.CloudCenter, painterConfig.LayoutAlgorithm);
            
            var sortedTagInfos = tagInfos
                .OrderByDescending(tagInfo => tagInfo.Frequency)
                .ToArray();

            if (sortedTagInfos.Length == 0)
                return Result.Fail<None>(new ArgumentException("Не нашлось ни одного тега для отрисовки"));
            
            var maxFrequency = sortedTagInfos.First().Frequency;
            var minFrequency = sortedTagInfos.Last().Frequency;
            
            foreach (var tagInfo in sortedTagInfos)
            {
                var tagFontSize = GetTagFontSize(tagInfo, painterConfig, maxFrequency, minFrequency); 
                var result = layouter.PutNextRectangle(GetTagSize(tagInfo, tagFontSize, painterConfig.FontFamily))
                    .Then(rec => canvas.Draw(tagInfo.Tag.Content,new Font(painterConfig.FontFamily, tagFontSize), rec, GetRandomBrush(painterConfig.Pallet)));

                if (!result.IsSuccess)
                    return result;
            }

            return Result.Ok();
        }
    }
}