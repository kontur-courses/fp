using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using TagsCloudContainer.Filters;
using TagsCloudContainer.RectangleGenerator;
using TagsCloudContainer.TokensGenerator;
using TagsCloudContainer.Visualization;

namespace TagsCloudContainer
{
    public class TagCloudVisualizator
    {
        private readonly ITokensParser tokensParser;
        private readonly IFilter filter;
        private readonly IRectangleGenerator rectangleGenerator;
        private IVisualizer visualizer;

        public TagCloudVisualizator(ITokensParser tokensParser, IFilter filter,
            IRectangleGenerator rectangleGenerator, IVisualizer visualizer)
        {
            this.tokensParser = tokensParser;
            this.filter = filter;
            this.rectangleGenerator = rectangleGenerator;
            this.visualizer = visualizer;
        }

        public Result<Bitmap> DrawTagCloud(string text, ICloudSetting setting)
        {
            visualizer.Clear();
            return Result.Ok(text)
                .Then(tokensParser.GetTokens)
                .Then(filter.Filtering)
                .Then(el => el.Where(w => w.Length > 3))
                .Then(CreateTokens)
                .Then(el => el.OrderByDescending(t => t.Count))
                .Then(el => el.ToArray())
                .Then(el => CreateRectangles(el, setting))
                .Then(el =>
                {
                    foreach (var tagRectangle in el)
                    {
                        visualizer.DrawTag(tagRectangle);
                    }
                })
                .Then(el => visualizer.Save());
        }


        private IEnumerable<TagRectangle> CreateRectangles(IList<Token> tokens, ICloudSetting setting)
        {
            var font = setting.Font;
            if (tokens.Count == 0)
            {
                yield break;
            }

            var maxCount = tokens[0].Count;
            foreach (var token in tokens)
            {
                font = new Font(font.FontFamily,
                    (int) Math.Max(((double) token.Count / maxCount) * setting.Font.Size, 9f));
                var rect = rectangleGenerator.PutNextRectangle(TextRenderer.MeasureText(token.Value, font));
                yield return new TagRectangle(token.Value, rect, font);
            }
        }

        private static IEnumerable<Token> CreateTokens(IEnumerable<string> tokens)
        {
            var countToken = new Dictionary<string, int>();
            foreach (var token in tokens)
            {
                if (countToken.ContainsKey(token))
                {
                    countToken[token]++;
                }
                else
                {
                    countToken[token] = 1;
                }
            }

            return countToken.Select(token => new Token(token.Key, (uint) token.Value));
        }

        private class Token
        {
            public string Value { get; }
            public uint Count { get; }

            public Token(string value, uint count)
            {
                Value = value;
                Count = count;
            }
        }
    }
}