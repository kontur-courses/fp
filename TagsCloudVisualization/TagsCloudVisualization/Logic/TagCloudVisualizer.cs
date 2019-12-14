﻿using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using ErrorHandler;
using TagsCloudVisualization.Logic.Painter;
using TagsCloudVisualization.Services;

namespace TagsCloudVisualization.Logic
{
    public class TagCloudVisualizer : IVisualizer
    {
        private readonly ILayouter layouter;
        private readonly IParser textParser;
        private readonly IImageGenerator imageGenerator;
        private readonly ITagPainter painter;
        private readonly IImageSettingsProvider imageSettingsProvider;
        private readonly Graphics measureGraphics;

        public TagCloudVisualizer(
            IImageGenerator imageGenerator,
            IParser textParser,
            ILayouter layouter,
            ITagPainter painter,
            IImageSettingsProvider imageSettingsProvider)
        {
            measureGraphics = Graphics.FromImage(new Bitmap(1, 1));
            this.imageGenerator = imageGenerator;
            this.imageSettingsProvider = imageSettingsProvider;
            this.textParser = textParser;
            this.layouter = layouter;
            this.painter = painter;
        }

        public Result<Bitmap> VisualizeTextFromFile(string fileName)
        {
            var imageCenter = new Point(
                imageSettingsProvider.ImageSettings.ImageSize.Width / 2,
                imageSettingsProvider.ImageSettings.ImageSize.Height / 2
            );
            var result = TextRetriever
                .RetrieveTextFromFile(fileName)
                .Then(text => textParser.ParseToTokens(text))
                .Then(tokens => CreateTagsFromTokens(tokens, imageCenter))
                .Then(tags => imageGenerator.CreateImage(tags));

            layouter.Reset();
            return result.RefineError("while trying to visualize image");
        }

        private Size CalculateWordSize(WordToken wordToken, Font font)
        {
            var size = measureGraphics.MeasureString(wordToken.Word, font);
            return size.ToSize();
        }

        private Result<IEnumerable<Tag>> CreateTagsFromTokens(IEnumerable<WordToken> wordTokens, Point imageCenter)
        {
            var tags = new List<Tag>();
            foreach (var token in wordTokens)
            {
                var tag = CreateTag(token, imageCenter);
                if (!tag.IsSuccess)
                    return Result.Fail<IEnumerable<Tag>>(tag.Error);
                tags.Add(tag.GetValueOrThrow());
            }
            return tags;
        }

        private Result<Tag> CreateTag(WordToken wordToken, Point imageCenter)
        {
            var color = painter.GetTagColor();
            var fontSize = CalculateFontSize(wordToken, imageSettingsProvider.ImageSettings);
            var wordFont = new Font(
                imageSettingsProvider.ImageSettings.Font.FontFamily,
                fontSize,
                imageSettingsProvider.ImageSettings.Font.Style
            );
            return layouter
                .PutNextRectangle(CalculateWordSize(wordToken, wordFont))
                .Then(wordRectangle =>
                    {
                        wordRectangle.Location = new Point(
                            wordRectangle.X + imageCenter.X,
                            wordRectangle.Y + imageCenter.Y
                        );
                        return new Tag(wordToken, wordRectangle, fontSize, color);
                    }
                );
        }

        private float CalculateFontSize(WordToken word, ImageSettings imageSettings)
        {
            return imageSettings.Font.Size + word.TextCount * 3;
        }
    }
}