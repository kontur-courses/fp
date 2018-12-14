using System.Collections.Generic;

namespace TagCloud.TagCloudVisualization.Analyzer
{
    public interface IWordAnalyzer
    {
        Dictionary<string, int> WeightWords(IEnumerable<string> words);
        IEnumerable<string> SplitWords(string fileContent);
    }
}