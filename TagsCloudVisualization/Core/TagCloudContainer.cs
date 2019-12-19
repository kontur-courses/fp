using System.Drawing;
using System.Linq;
using TagsCloudVisualization.Drawers;
using TagsCloudVisualization.Layouters;
using TagsCloudVisualization.Painters;
using TagsCloudVisualization.Preprocessing;
using TagsCloudVisualization.Text;
using TagsCloudVisualization.Utils;
using TagsCloudVisualization.WordStatistics;

namespace TagsCloudVisualization.Core
{
    public class TagCloudContainer
    {
        private readonly WordDrawer drawer;
        private readonly WordLayouter layouter;
        private readonly WordPainter painter;
        private readonly InputPreprocessor preprocessor;
        private readonly StatisticsCounter statCounter;
        private readonly ITextReader[] textReaders;

        public TagCloudContainer(ITextReader[] textReaders, InputPreprocessor preprocessor,
            StatisticsCounter statCounter, WordLayouter layouter,
            WordPainter painter, WordDrawer drawer)
        {
            this.textReaders = textReaders;
            this.preprocessor = preprocessor;
            this.statCounter = statCounter;
            this.layouter = layouter;
            this.painter = painter;
            this.drawer = drawer;
        }

        public Result<Bitmap> GetTagCloud(string filepath)
        {
            return FindTextReader(filepath)
                .Then(textReader => textReader.GetAllWords(filepath))
                .Then(words => preprocessor.PreprocessWords(words))
                .Then(preprocessedWords => statCounter.GetAnalyzedText(preprocessedWords))
                .Then(analyzedText => layouter.GetLayoutedText(analyzedText))
                .Then(analyzedLayoutedText => painter.GetPaintedWords(analyzedLayoutedText))
                .Then(paintedWords => drawer.GetDrawnLayoutedWords(paintedWords));
        }

        private Result<ITextReader> FindTextReader(string filepath)
        {
            if (filepath == null)
                return ResultExt.Fail<ITextReader>("Text file was not specified");
            if(!filepath.Contains('.'))
                return ResultExt.Fail<ITextReader>("File doesn't contain format information!");
            var format = filepath.Split('.').Last().ToLower();
            var reader = textReaders.FirstOrDefault(x => x.Formats.Contains(format));
            if (reader == null)
                return ResultExt.Fail<ITextReader>($"{format.ToUpper()} format is not supported");
            return reader.AsResult();
        }
    }
}