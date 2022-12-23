using System.Collections.Generic;
using TagsCloudVisualization.Infrastructure;
using TagsCloudVisualization.Infrastructure.Analyzer;

namespace TagsCloudVisualization.DefinerFontSize
{
    public interface IDefinerFontSize
    {
        Result<IEnumerable<WordWithFont>> DefineFontSize(IEnumerable<IWeightedWord> words);
    }
}