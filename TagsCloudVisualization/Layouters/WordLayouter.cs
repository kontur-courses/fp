using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using TagsCloudVisualization.Core;
using TagsCloudVisualization.Layouters.CloudLayouters;
using TagsCloudVisualization.Utils;
using TagsCloudVisualization.WordStatistics;

namespace TagsCloudVisualization.Layouters
{
    public class WordLayouter
    {
        private readonly CloudLayouterConfiguration cloudLayouterConfiguration;
        private readonly IWordSizeChooser sizeChooser;

        public WordLayouter(CloudLayouterConfiguration cloudConfiguration, IWordSizeChooser sizeChooser)
        {
            cloudLayouterConfiguration = cloudConfiguration;
            this.sizeChooser = sizeChooser;
        }

        public Result<AnalyzedLayoutedText> GetLayoutedText(AnalyzedText analyzedText)
        {
            var getLayouterResult = ResultExt.Of(() => cloudLayouterConfiguration.GetCloudLayouter());
            if (!getLayouterResult.IsSuccess)
                return ResultExt.Fail<AnalyzedLayoutedText>(getLayouterResult.Error);
            var getSizesResult = ResultExt.Of(() => sizeChooser.GetWordSizes(analyzedText, 20, 20));
            if (!getSizesResult.IsSuccess)
                return ResultExt.Fail<AnalyzedLayoutedText>(getSizesResult.Error);
            var layout = new Dictionary<Word, Rectangle>();
            foreach (var wordSizePair in getSizesResult.Value.OrderByDescending(x => x.Value.Width))
            {
                var putRectangleResult = ResultExt.Of(() => getLayouterResult.Value
                        .PutNextRectangle(wordSizePair.Value));
                if (!putRectangleResult.IsSuccess)
                    return ResultExt.Fail<AnalyzedLayoutedText>(putRectangleResult.Error);
                layout[wordSizePair.Key] = putRectangleResult.Value;
            }
            return analyzedText.ToLayoutedText(layout
                .Select(x => new LayoutedWord(x.Key, x.Value))
                .ToArray()).AsResult();
        }
    }
}