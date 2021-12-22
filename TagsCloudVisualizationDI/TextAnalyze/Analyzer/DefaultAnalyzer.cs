using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace TagsCloudVisualizationDI.TextAnalyze.Analyzer
{
    public class DefaultAnalyzer : IAnalyzer
    {
        private readonly HashSet<PartsOfSpeech.SpeechPart> _excludedSpeechParts;
        private readonly IEnumerable<string> _excludedWords;
        public string FilePath { get; }
        public string SaveAnalyzePath { get; }
        public string MystemPath { get; }
        public string MystemArgs { get; }


        public DefaultAnalyzer(IEnumerable<PartsOfSpeech.SpeechPart> excludedSpeechParts, IEnumerable<string> excludedWords,
            string filePath, string saveAnalyzePath, string mystemPath, string arguments)
        {
            _excludedSpeechParts = excludedSpeechParts.ToHashSet();
            _excludedWords = excludedWords;
            FilePath = filePath;
            SaveAnalyzePath = saveAnalyzePath;
            MystemPath = mystemPath;
            MystemArgs = arguments;
        }


        private static bool CheckWord(string inputWord, out string wordContent, out PartsOfSpeech.SpeechPart enumElementOfCurrentType)
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
            enumElementOfCurrentType = (PartsOfSpeech.SpeechPart)Enum.Parse(typeof(PartsOfSpeech.SpeechPart), type);
            return (inputWord.Split(' ').Length == 1);
        }

        private bool IsNotExcludedPart(PartsOfSpeech.SpeechPart enumElementOfCurrentType)
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
                if (CheckWord(word, out string content, out PartsOfSpeech.SpeechPart type))
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
            Console.WriteLine(File.Exists(FilePath));

            if (!File.Exists(FilePath))
            {
                Console.WriteLine("!!!");
                return Result.Fail<None>($"filepath is not correct: {FilePath}");
            }
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
            


            /*
            var process = Process.Start(new ProcessStartInfo
            {
                FileName = Checker.ResultOfGetPathToFile(MystemPath).GetValueOrThrow(),
                Arguments = MystemArgs + ' ' + Checker.ResultOfGetPathToFile(FilePath) + ' ' + Checker.ResultOfGetPathToFile(SaveAnalyzePath),
            });
            process.WaitForExit();
            */
        }

        public Result<None> InvokeMystemAnalizationResult()
        {
            {
                Console.WriteLine("!2");
                var invokeResult = InvokeMystemAnalyze();
                Console.WriteLine(invokeResult.Error);
                Console.WriteLine(invokeResult.IsSuccess);
                Console.WriteLine(invokeResult.Value);
                return invokeResult;
            }
        }
    }
}
