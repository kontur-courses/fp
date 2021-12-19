using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace TagsCloudVisualizationDI.TextAnalization.Analyzer
{
    public interface IAnalyzer
    {
        public string FilePath { get; }
        public string MystemArgs { get; }
        public string SaveAnalizationPath { get; }
        public string MystemPath { get; }

        IEnumerable<Word> GetAnalyzedWords(IEnumerable<string> words);

        public Result<None> InvokeMystemAnalizationResult()
        {
            var invokeResult = Result.OfAction(() => InvokeMystemAnalization());
            return invokeResult;
        }

        private void InvokeMystemAnalization()
        {
            var process = Process.Start(new ProcessStartInfo
            {
                FileName = MystemPath,
                Arguments = MystemArgs + ' ' + FilePath + ' ' + SaveAnalizationPath,
            });

            process.WaitForExit();
        }
    }
}
