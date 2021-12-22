using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace TagsCloudVisualizationDI.TextAnalyze.Analyzer
{
    public class DefaultAnalyzer : IAnalyzer
    {
        private readonly HashSet<SpeechPart> _excludedSpeechParts;
        private readonly IEnumerable<string> _excludedWords;
        public string FilePath { get; }
        public string SaveAnalyzePath { get; }
        public string MystemPath { get; }
        public string MystemArgs { get; }


        public DefaultAnalyzer(IEnumerable<SpeechPart> excludedSpeechParts, IEnumerable<string> excludedWords,
            string filePath, string saveAnalyzePath, string mystemPath, string arguments)
        {
            _excludedSpeechParts = excludedSpeechParts.ToHashSet();
            _excludedWords = excludedWords;
            FilePath = filePath;
            SaveAnalyzePath = saveAnalyzePath;
            MystemPath = mystemPath;
            MystemArgs = arguments;
        }


        private static bool CheckWord(string inputWord, out string wordContent, out SpeechPart enumElementOfCurrentType)
        {
            var wordAndPart = inputWord.Split(new[] { ' ', ',', '=' }, 3, StringSplitOptions.RemoveEmptyEntries);
            if (wordAndPart.Length < 2)
            {
                wordContent = default;
                enumElementOfCurrentType = default;
                return false;
            }

            wordContent = wordAndPart[0];
            var type = wordAndPart[1];
            enumElementOfCurrentType = (SpeechPart)Enum.Parse(typeof(SpeechPart), type);
            return (inputWord.Split(' ').Length == 1);
        }

        private bool IsNotExcludedPart(SpeechPart enumElementOfCurrentType)
        {
            var excludedParts = _excludedSpeechParts;
            if (!excludedParts.Contains(enumElementOfCurrentType))
                return true;

            return false;
        }

        public IEnumerable<Word> GetAnalyzedWords(IEnumerable<string> words)
        {

            foreach (var word in words)
            {
                if (CheckWord(word, out var content, out var type))
                {
                    if (IsNotExcludedPart(type) && IsNotExcludedWord(content))
                        yield return new Word(content);
                }
            }
        }

        private bool IsNotExcludedWord(string word)
        {
            return !(_excludedWords.Contains(word));
        }

        public Result<None> InvokeMystemAnalyze()
        {

            if (!File.Exists(FilePath))
                return Result.Fail<None>($"filepath is not correct: {FilePath}");
            if (!File.Exists(SaveAnalyzePath))
                return Result.Fail<None>($"path to temp Document is not correct: {SaveAnalyzePath}");
            if (!File.Exists(MystemPath))
                return Result.Fail<None>($"path to mystem is not correct: {MystemPath}");
            
            var process = Process.Start(new ProcessStartInfo
            {
                FileName = MystemPath,
                Arguments = MystemArgs + ' ' + FilePath + ' ' + SaveAnalyzePath,
            });
            process.WaitForExit();
            return Result.Ok();
        }

        public Result<None> InvokeMystemAnalizationResult()
        {
            return InvokeMystemAnalyze();
        }
    }
}
