using System.Collections.Generic;
using TagsCloudVisualization.Results;
using TagsCloudVisualization.Structures;

namespace TagsCloudVisualization.WordAnalyzers
{
    public interface IMorphAnalyzer
    {
        IEnumerable<Result<WordInfo>> AnalyzeText(string text);
        string DefinePartOfSpeech(string word);
        string GetStandardForm(string word);
    }
}
