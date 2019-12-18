using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using TagsCloudResult.Infrastructure.Common;

namespace TagsCloudResult
{
    public static class Compositor
    {
        public static Result<IEnumerable<(Rectangle, LayoutWord)>> Composite(IEnumerable<LayoutWord> layoutWords,
            Func<Size, Rectangle> putNextRectangle, AppSettings settings)
        {
            var words = new HashSet<(Rectangle, LayoutWord)>();

            if (settings.AlgorithmsSettings.Centering)
                layoutWords = layoutWords.OrderBy(x => -x.Size.Width * x.Size.Height);
            try
            {
                foreach (var layoutWord in layoutWords)
                {
                    var rectangle = putNextRectangle(layoutWord.Size);
                    words.Add((rectangle, layoutWord));
                }
            }
            catch (Exception e)
            {
                return Result.Fail<IEnumerable<(Rectangle, LayoutWord)>>(e.Message);
            }

            return words;
        }
    }
}