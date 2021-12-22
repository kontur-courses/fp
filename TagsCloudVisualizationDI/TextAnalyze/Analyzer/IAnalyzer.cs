using System.Collections.Generic;

namespace TagsCloudVisualizationDI.TextAnalyze.Analyzer
{
    public interface IAnalyzer
    {
        public string FilePath { get; }
        public string MystemArgs { get; }
        public string SaveAnalyzePath { get; }
        public string MystemPath { get; }

        IEnumerable<Word> GetAnalyzedWords(IEnumerable<string> words);

        public Result<None> InvokeMystemAnalizationResult();
        

        Result<None> InvokeMystemAnalyze();
    }
}
