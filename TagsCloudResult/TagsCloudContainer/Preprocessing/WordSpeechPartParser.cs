using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using TagsCloudContainer.Results;

namespace TagsCloudContainer.Preprocessing
{
    public class WordSpeechPartParser : IWordSpeechPartParser
    {
        private static readonly Regex speechPartRegex = new(@".*?=(?'SpeechPart'\w+)");
        private static readonly Regex validWordRegex = new(@"^[а-я-]+$", RegexOptions.IgnoreCase);

        private static readonly ProcessStartInfo myStemStartInfo = new()
        {
            FileName = "executables/mystem.exe",
            Arguments = "-n -i -l -e cp866",
            RedirectStandardInput = true,
            RedirectStandardOutput = true
        };

        public Result<IEnumerable<SpeechPartWord>> ParseWords(IEnumerable<string> words)
        {
            using var myStem = Process.Start(myStemStartInfo);
            return myStem != null
                ? ParseWords(words, myStem)
                : Result.Fail<IEnumerable<SpeechPartWord>>(
                    $"Can't start mystem. Executable path: {myStemStartInfo.FileName}");
        }

        private static Result<IEnumerable<SpeechPartWord>> ParseWords(IEnumerable<string> words, Process myStem)
        {
            return FilterWords(words)
                .Select(word =>
                    GetWordInfo(word, myStem)
                        .Then(ParseSpeechPart)
                        .Then(speechPart => new SpeechPartWord(word, speechPart))
                        .RefineError($"Word: {word}"))
                .CombineResults();
        }

        private static IEnumerable<string> FilterWords(IEnumerable<string> words) =>
            words.Where(word => validWordRegex.IsMatch(word));

        private static Result<string> GetWordInfo(string word, Process myStem)
        {
            myStem.StandardInput.WriteLine(word);
            var readTask = myStem.StandardOutput.ReadLineAsync();
            var canProcessWord = readTask.Wait(450);
            if (!canProcessWord || readTask.Result == null)
                return Result.Fail<string>("Can't get result from mystem");

            return readTask.Result;
        }

        private static Result<SpeechPart> ParseSpeechPart(string wordInfo)
        {
            var speechPartGroup = speechPartRegex.Match(wordInfo).Groups["SpeechPart"];
            if (speechPartGroup.Success && Enum.TryParse<SpeechPart>(speechPartGroup.Value, out var speechPart))
                return speechPart;

            return Result.Fail<SpeechPart>("Can't parse speech part");
        }
    }
}