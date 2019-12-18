using SyntaxTextParser;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using ResultPattern;
using TagsCloudGenerator.CloudPrepossessing;

namespace TagsCloudGenerator
{
    public static class TagGenerator
    {
        public static Result<List<CloudTag>> CreateCloudTags(string fullPath, TextParser parser,
            ITagsPrepossessing tagPlacer, CloudFormat cloudFormat)
        {
            var parsedElements = parser.ParseElementsFromFile(fullPath);
            if (!parsedElements.IsSuccess)
                return Result.Fail<List<CloudTag>>(parsedElements.Error);

            var elements = parsedElements.GetValueOrThrow();
            elements = cloudFormat.TagOrderPreform.OrderEnumerable(elements);
            
            var result = new List<CloudTag>();
            foreach (var element in elements)
            {
                var font = new Font(cloudFormat.TagTextFontFamily,
                    Math.Min(cloudFormat.MaximalFontSize, element.Count * cloudFormat.FontSizeMultiplier));
                var size = new Size(TextRenderer.MeasureText(element.Element, font).Width,
                    TextRenderer.MeasureText(element.Element, font).Height);

                var rect = tagPlacer.PutNextRectangle(size);
                if (!rect.IsSuccess)
                    return Result.Fail<List<CloudTag>>(rect.Error);

                result.Add(new CloudTag(rect.GetValueOrThrow(), element.Element,
                    cloudFormat.TagTextFormat, font));
            }

            return result.AsResult();
        }
    }
}