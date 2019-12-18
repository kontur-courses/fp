using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using ResultOf;

namespace TagsCloudContainer.MyStem
{
    public class MyStemResultParser
    {
        public Result<IEnumerable<(string, string)>> GetPartsOfSpeechByResultOfNiCommand(string myStemResult,
            IEnumerable<string> words)
        {
            return Result.OfCollection(words.Select(w => GetPartOfSpeechForWord(myStemResult, w).AddToPairFromLeft(w))
                .ToList());
        }

        public Result<IEnumerable<(string, string)>> GetInitialFormsByResultOfNiCommand(string myStemResult,
            IEnumerable<string> words)
        {
            return Result.OfCollection(words.Select(w => GetInitialFormForWord(myStemResult, w).AddToPairFromLeft(w))
                .ToList());
        }

        private Result<string> GetPartOfSpeechForWord(string myStemResult, string word)
        {
            var partOfSpeechRegex = new Regex($@"(?:^|\s){word}{{.+?=(\w+)[,|=]");
            return GetInformationForWord(myStemResult, word, partOfSpeechRegex);
        }

        private Result<string> GetInitialFormForWord(string myStemResult, string word)
        {
            var initialFormRegex = new Regex($@"(?:^|\s){word}{{(\w+)");
            return GetInformationForWord(myStemResult, word, initialFormRegex);
        }

        private Result<string> GetInformationForWord(string myStemResult, string word, Regex informationRegex)
        {
            var match = informationRegex.Match(myStemResult);
            var matchGroups = match.Groups;
            return matchGroups.Count < 2
                ? Result.Fail<string>($"{nameof(myStemResult)} does not contain result for {word}")
                : matchGroups[1].Value;
        }
    }
}