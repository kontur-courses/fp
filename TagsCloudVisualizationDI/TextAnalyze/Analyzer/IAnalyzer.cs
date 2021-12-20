using System.Collections.Generic;
using System.Diagnostics;

namespace TagsCloudVisualizationDI.TextAnalyze.Analyzer
{
    public interface IAnalyzer
    {
        public string FilePath { get; }
        public string MystemArgs { get; }
        public string SaveAnalyzePath { get; }
        public string MystemPath { get; }

        IEnumerable<Word> GetAnalyzedWords(IEnumerable<string> words);

        public Result<None> InvokeMystemAnalizationResult()
        {
            var invokeResult = Result.OfAction(() => InvokeMystemAnalyze());
            return invokeResult;
        }

        private void InvokeMystemAnalyze()
        {
            Checker.CheckPathToFile(FilePath);
            Checker.CheckPathToFile(MystemPath);
            Checker.CheckPathToFile(SaveAnalyzePath);

            var process = Process.Start(new ProcessStartInfo
            {
                FileName = MystemPath,
                Arguments = MystemArgs + ' ' + FilePath + ' ' + SaveAnalyzePath,
            });

            process.WaitForExit();
        }
    }
}
